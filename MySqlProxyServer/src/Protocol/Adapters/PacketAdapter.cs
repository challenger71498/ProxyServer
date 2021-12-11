// Copyright (c) Min. All rights reserved.

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

            this.Read(reader);
        }

        /// <inheritdoc/>
        public int PayloadLength { get; private set; }

        /// <inheritdoc/>
        public int SequenceId { get; private set; }

        /// <inheritdoc/>
        public byte[] Payload { get; private set; }

        public void Read(BinaryReader reader)
        {
            this.PayloadLength = reader.ReadFixedInt(3);
            this.SequenceId = reader.ReadFixedInt(1);

            if (this.PayloadLength == 0)
            {
                return;
            }

            this.Payload = new byte[this.PayloadLength];
            reader.Read(this.Payload);
        }
    }
}
