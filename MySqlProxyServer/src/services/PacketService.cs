// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    /// <summary>
    /// Convert binary packet data to a payload.
    /// </summary>
    public class PacketService
    {
        private readonly Queue<byte[]> payloadQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketService"/> class.
        /// </summary>
        public PacketService()
        {
            this.payloadQueue = new Queue<byte[]>();
        }

        /// <summary>
        /// Event handler on payload arrive.
        /// </summary>
        public event EventHandler<byte[]> OnPayload;

        /// <summary>
        /// Pushes binary packet data to the queue.
        /// </summary>
        /// <param name="binaryPacket">Packet to push.</param>
        public void PushPacket(byte[] binaryPacket)
        {
            var packet = new PacketAdapter(binaryPacket);

            this.payloadQueue.Enqueue(packet.Payload);

            if (packet.PayloadLength == 0xffffff)
            {
                return;
            }

            var payload = this.FlushQueue();
            this.OnPayload?.Invoke(this, payload);
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
}
