// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    /// <summary>
    /// Convert binary packet data to a payload.
    /// </summary>
    public class PacketService
    {
        private readonly Queue<byte[]> payloadQueue;
        private int initialSequenceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketService"/> class.
        /// </summary>
        public PacketService()
        {
            this.payloadQueue = new Queue<byte[]>();

            this.WhenPayloadFlushed = Observable.Empty<PayloadFlushedArgs>();
        }

        /// <summary>
        /// Event handler on payload arrive.
        /// </summary>
        public IObservable<PayloadFlushedArgs> WhenPayloadFlushed { get; private set; }

        public byte[][] GetPackets(int sequenceId, byte[] payload)
        {
            var output = new List<byte[]>();

            var buffer = payload;
            var id = sequenceId;

            while (buffer.Length >= 2E24 - 1)
            {
                id += 1;

                var length = (1024 * 1024 * 16) - 1;

                var split = payload[..length];
                buffer = payload[(length + 1)..];

                var packet = new Packet(split.Length, id, split);
                output.Add(ToBinary(packet));
            }

            var last = new Packet(buffer.Length, id, buffer);
            output.Add(ToBinary(last));

            return output.ToArray();
        }

        /// <summary>
        /// Pushes binary packet data to the queue.
        /// </summary>
        /// <param name="packet">Packet to push.</param>
        public void PushPacket(IPacket packet)
        {
            if (this.payloadQueue.Count == 0)
            {
                this.initialSequenceId = packet.SequenceId;
            }

            // TODO: Refactor with IObservable.Buffer().
            this.payloadQueue.Enqueue(packet.Payload);

            if (packet.PayloadLength == 0xffffff)
            {
                return;
            }

            var payload = this.FlushQueue();
            var payloadFlushedArgs = new PayloadFlushedArgs(this.initialSequenceId, payload);

            this.WhenPayloadFlushed.Append(payloadFlushedArgs);
        }

        private static byte[] ToBinary(IPacket packet)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(packet.PayloadLength, 3);
            writer.WriteFixedInt(packet.SequenceId, 1);
            writer.Write(packet.Payload);

            Console.WriteLine($"STREAM_LEN: {stream.Length}");

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }

        private byte[] FlushQueue()
        {
            var output = new List<byte>();

            while (this.payloadQueue.Count > 0)
            {
                var binary = this.payloadQueue.Dequeue();
                output.AddRange(binary);
            }

            return output.ToArray();
        }
    }

    public class PayloadFlushedArgs
    {
        public PayloadFlushedArgs(int initialSequenceId, byte[] payload)
        {
            this.InitialSequenceId = initialSequenceId;
            this.Payload = payload;
        }

        public int InitialSequenceId { get; set; }

        public byte[] Payload { get; set; }
    }
}
