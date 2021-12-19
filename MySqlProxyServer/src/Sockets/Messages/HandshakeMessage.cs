// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class HandshakeMessage : IProtocolMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeMessage"/> class.
        /// </summary>
        /// <param name="packets">Array of packets.</param>
        public HandshakeMessage(Handshake handshake)
        {
            this.Protocol = handshake;
        }

        /// <inheritdoc/>
        public SocketControllerMessageType Type { get; } = SocketControllerMessageType.HANDSHAKE;

        /// <summary>
        /// Gets or sets a list of packet.
        /// </summary>
        /// <value>Array of packets.</value>
        public IProtocol Protocol { get; }
    }
}
