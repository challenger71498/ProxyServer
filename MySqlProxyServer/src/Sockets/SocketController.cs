// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    // TODO: Move away dummy IPayloadService interface.
    public interface IPayloadService
    {
        IProtocol? TryGetProtocol(byte[] payload);
    }

    // TODO: Move away dummy IPacketService interface.
    public interface IPacketService
    {
        IObservable<ISocketControllerMessage> Pipe(IObservable<byte[]> yay);
    }

    public class SocketController : ISocketController
    {
        private readonly ISocketConnection connection;
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;


        public SocketController(
            ISocketConnection connection,
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.connection = connection;
            this.packetService = packetService;
            this.payloadService = payloadService;

            this.WhenMessageCreated = this.GetMessageStream(this.connection.WhenDataReceived);
        }

        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }

        // NOTE: Create MessageHandler?
        private IObservable<ISocketControllerMessage> GetMessageStream(IObservable<byte[]> dataStream)
        {
            byte[]? captured = null;

            // NOTE: To catch exception from every pipe, observables should be connected by operator.
            // NOTE: Cannot seperate observables by variables. (ex: fooStream, barStream, bazStream, ...)
            var messageStream = dataStream
                .Do(data => captured = data)
                .Select(this.RawDataPipe)
                .Select(this.PacketPipe)
                .Select(this.PayloadPipe)
                .Select(this.ProtocolPipe)
                .Catch((SocketControllerException e) =>
                {
                    if (captured == null)
                    {
                        throw new NullReferenceException("Captured data is null. This should never be happened.");
                    }

                    var rawMessage = new RawDataMessage(captured);
                    return Observable.ToObservable(new[] { rawMessage });
                })
                .Catch((Exception e) =>
                {
                    throw e;
                })
                .Repeat();

            return messageStream;
        }

        IPacket RawDataPipe(byte[] data)
        {
            var packet = PacketFactory.TryCreatePacket(data);

            if (packet != null)
            {
                return packet;
            }

            Console.WriteLine("Failed to get packet. Streaming directly to the messenger...");
            throw new SocketControllerException();
        }

        byte[] PacketPipe(IPacket packet)
        {
            // Get payload from packet.
            // Throw SocketControllerException if failed.
            return null;
        }

        IProtocol PayloadPipe(byte[] payload)
        {
            // Get protocol from packet.
            // Throw SocketControllerException if failed.
            return null;
        }

        ISocketControllerMessage ProtocolPipe(IProtocol protocolStream)
        {
            // Create message from the protocol.
            // Throw SocketControllerException if failed.
            return null;
        }

        private async void PayloadSelector(PayloadFlushedArgs e)
        {
            var info = new PayloadInfo { Payload = e.Payload, };

            if (this.payloadService != null)
            {
                var processed = await this.payloadService.TryProcess(e.Payload);

                if (processed != null)
                {
                    info = (PayloadInfo)processed;
                }
            }
        }

        private void Send(PayloadFlushedArgs e)
        {
            var packets = this.packetService.GetPackets(e.InitialSequenceId, info.Payload);

            if (!info.Loopback)
            {
                this.PacketsReadyEventHandler?.Invoke(sender, packets);
                return;
            }

            foreach (var packet in packets)
            {
                await this.connection.Send(packet);
            }
        }

        // NOTE: Is it okay to use internal exception?
        private class SocketControllerException : Exception
        {
            public SocketControllerException()
            : base("Internal socket controller exception.")
            {
            }
        }
    }
}
