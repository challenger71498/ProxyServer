// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Protocol
{
    /// <summary>
    /// MySQL packet interface.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// Gets the length of the payload.
        /// </summary>
        int PayloadLength { get; }

        /// <summary>
        /// Gets the id of the sequence.
        /// </summary>
        int SequenceId { get; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        byte[] Payload { get; }
    }
}
