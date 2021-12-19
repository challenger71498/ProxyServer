// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class HandshakeResponseMessage : IProtocolMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeResponseMessage"/> class.
        /// </summary>
        /// <param name="handshakeResponse">Handshake response protocol.</param>
        public HandshakeResponseMessage(HandshakeResponse handshakeResponse)
        {
            this.Protocol = handshakeResponse;
        }

        /// <inheritdoc/>
        public SocketControllerMessageType Type { get; } = SocketControllerMessageType.HANDSHAKE_RESPONSE;

        /// <summary>
        /// Gets or sets a handshake response protocol.
        /// </summary>
        /// <value>HandshakeResponse instance.</value>
        public IProtocol Protocol { get; }
    }
}
