// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class SocketController : ISocketController
    {
        private readonly ISocketConnection connection;
        private readonly MessageSender sender;
        private readonly MessageReceiver receiver;

        public SocketController(
            ISocketConnection connection,
            MessageSender sender,
            MessageReceiver receiver)
        {
            this.connection = connection;
            this.sender = sender;

            this.WhenMessageCreated = this.sender.GetMessageStream(this.connection.WhenDataReceived);
        }

        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }
    }

    // TODO: Move away dummy IPayloadService interface.
    public interface IPayloadService
    {
        IProtocol? TryGetProtocol(byte[] payload);
    }

    // TODO: Move away dummy IPacketService interface.
    public interface IPacketService
    {
        IObservable<IPayloadData> PipePacket(IObservable<IPacket> packetStream);

        IObservable<IPacket> PipePayload(int id, IObservable<byte[]> payload);
    }

    public class PacketService : IPacketService
    {
        public IObservable<IPacket> PipePayload(int id, IObservable<byte[]> payloadStream)
        {
            return payloadStream.SelectMany((payload) => this.GetPackets(id, payload));
        }

        public IObservable<IPayloadData> PipePacket(IObservable<IPacket> packetStream)
        {
            var payloadStream = packetStream
                .Buffer(packetStream.Where(this.IsPacketEOF))
                .Select(this.GetPayload);

            return payloadStream;
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
    }
}
