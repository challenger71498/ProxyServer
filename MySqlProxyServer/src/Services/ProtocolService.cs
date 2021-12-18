using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class ProtocolService : IProtocolService
    {
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;
        private readonly IProtocolFactory[] factories;

        public ProtocolService(
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;

            this.factories = Array.Empty<IProtocolFactory>();
        }

        public IObservable<IData> ToPayload(int initialSequenceId, IObservable<IData> dataStream)
        {
            return dataStream.Select(data =>
            {
                if (data is not IWritableProtocol protocol)
                {
                    return data;
                }

                var payload = protocol.ToPayload();

                return new PayloadData(initialSequenceId, payload);
            });
        }

        public IObservable<IData> FromPayload(IObservable<IData> dataStream)
        {
            return dataStream
                .GroupBy(data => data is IPayloadData)
                .SelectMany(stream =>
                {
                    // If not IPayloadData, skip.
                    if (!stream.Key)
                    {
                        return stream;
                    }

                    // If IPayloadData, try convert to IProtocol.
                    return stream
                        .Select(data => (IPayloadData)data)
                        .Let(this.TryConvertToProtocol);
                });
        }

        private IObservable<IData> TryConvertToProtocol(IObservable<IPayloadData> stream)
        {
            var protocolStream = stream
                .Select<IPayloadData, IData>(payloadData =>
                {
                    // Try create protocol by factories.
                    foreach (var factory in this.factories)
                    {
                        if (factory.TryCreate(payloadData.Payload, out var protocol))
                        {
                            return protocol;
                        }
                    }

                    // If failed, stream it as-is.
                    return payloadData;
                });

            return protocolStream
                .GroupBy(data => data is IPayloadData)
                .SelectMany<IGroupedObservable<bool, IData>, IData>(data =>
                {
                    // If not IPayloadData, skips.
                    if (!data.Key)
                    {
                        return data;
                    }

                    // If IPayloadData, switch back to raw data.
                    return data
                        .Let(this.payloadService.ToPacket)
                        .Let(this.packetService.ToBinaryData);
                });
        }
    }
}