// Copyright (c) Min. All rights reserved.

using System;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class SocketHandler
    {
        private readonly SocketConnection connection;
        private readonly PacketService packetService;
        private readonly PayloadService? payloadService;

        public SocketHandler(SocketConnection socketConnection, PacketService packetService, PayloadService? payloadService = null)
        {
            // Console.WriteLine("Socket handler has been created");

            this.connection = socketConnection;
            this.packetService = packetService;

            this.payloadService = payloadService;

            this.connection.DataReceivedEventHandler += this.OnDataReceived;
            this.connection.DisconnectedEventHandler += this.OnDisconnected;

            this.packetService.PayloadFlushedEventHandler += this.OnPayloadFlush;
        }

        public event EventHandler<EventArgs>? DisconnectedEventHandler;

        public event EventHandler<byte[][]>? PacketsReadyEventHandler;

        public async Task Send(byte[] data)
        {
            // Console.WriteLine($"Sending... {System.Text.Encoding.ASCII.GetString(data)}");

            await this.connection.Send(data);
        }

        public void Disconnect()
        {
            this.connection.Disconnect();
        }

        private void OnDataReceived(object? sender, byte[] binary)
        {
            // Console.WriteLine($"Data received! {binary.Length}");
            // Console.WriteLine(Convert.ToHexString(binary));

            var packet = PacketFactory.TryCreatePacket(binary);

            if (packet == null)
            {
                Console.WriteLine("Failed to get packet. Streaming directly to the event...");
                this.PacketsReadyEventHandler?.Invoke(this, new byte[][] { binary });
                return;
            }

            this.packetService.PushPacket(packet);
        }

        private async void OnPayloadFlush(object? sender, PayloadFlushedEventArgs e)
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

        private void OnDisconnected(object? sender, EventArgs e)
        {
            this.DisconnectedEventHandler?.Invoke(this, e);
        }
    }
}
