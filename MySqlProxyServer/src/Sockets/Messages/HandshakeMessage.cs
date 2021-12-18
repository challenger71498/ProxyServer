// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Sockets
{
    public class HandshakeMessage : ISocketControllerMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeMessage"/> class.
        /// </summary>
        /// <param name="packets">Array of packets.</param>
        public HandshakeMessage(byte[][] packets)
        {
            this.Packets = packets;
        }

        /// <inheritdoc/>
        public SocketControllerMessageType Type { get; } = SocketControllerMessageType.HANDSHAKE;

        /// <summary>
        /// Gets or sets a list of packet.
        /// </summary>
        /// <value>Array of packets.</value>
        public byte[][] Packets { get; set; }
    }
}
