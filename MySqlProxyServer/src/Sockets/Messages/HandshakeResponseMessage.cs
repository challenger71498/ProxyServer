// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class HandshakeResponseMessage : ISocketControllerMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeResponseMessage"/> class.
        /// </summary>
        /// <param name="protocol">Handshake response protocol.</param>
        public HandshakeResponseMessage(HandshakeResponse protocol)
        {
            this.Protocol = protocol;
        }

        /// <inheritdoc/>
        public SocketControllerMessageType Type { get; } = SocketControllerMessageType.HANDSHAKE_RESPONSE;

        /// <summary>
        /// Gets or sets a handshake response protocol.
        /// </summary>
        /// <value>HandshakeResponse instance.</value>
        public HandshakeResponse Protocol { get; set; }
    }
}
