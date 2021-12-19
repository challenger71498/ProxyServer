using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class PayloadService : IPayloadService
    {
        public IObservable<IData> ToPacket(IObservable<IData> dataStream)
        {
            return dataStream.SelectMany<IData, IData>((data) =>
            {
                if (data is not IPayloadData payloadData)
                {
                    return new[] { data };
                }

                return this.GetPackets(payloadData.InitialSequenceId, payloadData.Payload);
            });
        }

        public IObservable<IData> FromPacket(IObservable<IData> dataStream)
        {
            return dataStream
                .GroupBy(data => data is IPacket)
                .SelectMany(data =>
                {
                    if (!data.Key)
                    {
                        return (IObservable<IData>)data;
                    }

                    var packetStream = data
                        .Select(data => (IPacket)data);

                    return packetStream
                        .Buffer(packetStream.Where(this.IsPacketEOF))
                        .Select(this.GetPayload)
                        .Do(data => Console.WriteLine(data.InitialSequenceId));
                });
        }

        private IEnumerable<IPacket> GetPackets(int id, byte[] payload)
        {
            var output = new List<IPacket>();
            var buffer = payload;

            while (buffer.Length >= 2E24 - 1)
            {
                id += 1;

                var length = (1024 * 1024 * 16) - 1;

                var split = payload[..length];
                buffer = payload[(length + 1)..];

                var packet = new Packet(split.Length, id, split);
                output.Add(packet);
            }

            var last = new Packet(buffer.Length, id, buffer);
            output.Add(last);

            return output;
        }

        private bool IsPacketEOF(IPacket packet)
        {
            return packet.PayloadLength == 0xffffff;
        }

        private IPayloadData GetPayload(IList<IPacket> packets)
        {
            var buffer = new List<byte>();
            int id = -1;

            foreach (var packet in packets)
            {
                if (id == -1)
                {
                    id = packet.SequenceId;
                }

                buffer.AddRange(packet.Payload);
            }

            var payload = buffer.ToArray();

            var payloadData = new PayloadData(id, payload);
            return payloadData;
        }
    }
}

// var payloadData = dataStream
//     .Where(data => data.GetType() == typeof(IPayloadData))
//     .Select(data => (IPayloadData)data)
//     .Select<IPayloadData, IData>(payloadData =>
//     {
//         foreach (var factory in this.factories)
//         {
//             if (factory.TryCreate(payloadData.Payload, out var protocol))
//             {
//                 return protocol;
//             }
//         }

//         return payloadData;
//     });

// var failedData = payloadData
//     .Where(payload => payload.GetType() == typeof(IPayloadData))
//     .Let(this.packetService.ToRawData);

// return Observable.Merge(rawStream, payloadData);

// return dataStream.Let<IData, IData>(data =>
// {
//     // Filter only IPayloadData.
//     var rawData = data.Where(data => data.GetType() != typeof(IPayloadData));

//     var payloadData = data.Select(data => (IPayloadData)data);



//     // If failed, rollback to raw binary data.
//     var packets = this.GetPackets(payloadData.InitialSequenceId, payloadData.Payload);
//     var binaries = packets.Select(packet => ?);
// });