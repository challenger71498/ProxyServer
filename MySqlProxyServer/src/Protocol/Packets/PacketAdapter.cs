// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    /// <summary>
    /// PacketAdapter adapts binary data to a payload.
    /// </summary>
    public class PacketAdapter : IPacket
    {
        public PacketAdapter(byte[] binary)
        {
            using var stream = new MemoryStream(binary);
            using var reader = new BinaryReader(stream);

            this.PayloadLength = reader.ReadFixedInt(3);
            this.SequenceId = reader.ReadFixedInt(1);

            if (this.PayloadLength == 0)
            {
                this.Payload = Array.Empty<byte>();
                return;
            }

            this.Payload = reader.ReadBytes(this.PayloadLength);
        }

        public int PayloadLength { get; private set; }

        public int SequenceId { get; private set; }

        public byte[] Payload { get; private set; }
    }
}
