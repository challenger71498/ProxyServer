// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class SocketController : ISocketController
    {
        private readonly ISocketConnection connection;
        private readonly PacketService packetService;
        private readonly PayloadService? payloadService;


        public SocketController(
            ISocketConnection connection,
            PacketService packetService,
            PayloadService? payloadService)
        {
            this.connection = connection;
            this.packetService = packetService;
            this.payloadService = payloadService;

            this.connection.WhenDataReceived.Subscribe(this.OnDataReceived);

            var protocolStream = this.packetService.WhenPayloadFlushed
                .Select(this.ProtocolSelector);

            // TODO: Create success protocol stream and failure payload stream.

            // TODO: On success protocol stream, addtional process will be executed.

            // TODO: On failure payload stream, push it directly to the end pipe. 

            // TODO: After all, merge all observables to one last observable: WhenMessageCreated.
        }

        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }

        private void OnDataReceived(byte[] binary)
        {
            // Console.WriteLine($"Data received! {binary.Length}");
            // Console.WriteLine(Convert.ToHexString(binary));

            var packet = PacketFactory.TryCreatePacket(binary);

            if (packet != null)
            {
                this.packetService.PushPacket(packet);
            }

            Console.WriteLine("Failed to get packet. Streaming directly...");

            var message = new RawDataMessage(binary);
            this.WhenMessageCreated.Append(message);

            return;
        }

        private IProtocol? ProtocolSelector(PayloadFlushedArgs e)
        {

        }

        private async void OnPayloadFlush(PayloadFlushedArgs e)
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
    }
}
