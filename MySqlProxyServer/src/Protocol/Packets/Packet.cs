// Copyright (c) Min. All rights reserved.

using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class Packet : IPacket
    {
        public int PayloadLength { get; set; }

        public int SequenceId { get; set; }

        public byte[] Payload { get; set; }

        public Packet(int payloadLength, int sequenceId, byte[] payload)
        {
            this.PayloadLength = payloadLength;
            this.SequenceId = sequenceId;
            this.Payload = payload;
        }
    }
}
