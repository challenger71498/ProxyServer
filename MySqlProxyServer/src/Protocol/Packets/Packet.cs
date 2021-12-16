// Copyright (c) Min. All rights reserved.

using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    /// <summary>
    /// <see cref="Packet"/> describes MySQL base packet.
    /// </summary>
    public class Packet : IPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="payloadLength">Length of the payload binary.</param>
        /// <param name="sequenceId">Sequence ID of the packet.</param>
        /// <param name="payload">Binary payload.</param>
        public Packet(int payloadLength, int sequenceId, byte[] payload)
        {
            this.PayloadLength = payloadLength;
            this.SequenceId = sequenceId;
            this.Payload = payload;
        }

        /// <summary>
        /// Gets or sets the length of the payload binary.
        /// </summary>
        /// <value>Payload binary length as int.</value>
        public int PayloadLength { get; set; }

        /// <summary>
        /// Gets or sets the sequence ID of the packet.
        /// </summary>
        /// <value>Sequence ID as int.</value>
        public int SequenceId { get; set; }

        /// <summary>
        /// Gets or sets the payload binary of the packet.
        /// </summary>
        /// <value>Payload as byte array.</value>
        public byte[] Payload { get; set; }
    }
}
