// Copyright (c) Min. All rights reserved.

using System;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public interface ISocketController
    {
        IObservable<ISocketControllerMessage> WhenMessageCreated { get; }
    }

    public interface ISocketControllerMessage
    {
        /// <summary>
        /// Gets the type of the socket controller message.
        /// </summary>
        /// <value>An enum value.</value>
        SocketControllerMessageType Type { get; }
    }

    public class RawDataMessage : ISocketControllerMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawDataMessage"/> class.
        /// </summary>
        /// <param name="raw">Array of packets.</param>
        public RawDataMessage(byte[] raw)
        {
            this.Raw = raw;
        }

        /// <inheritdoc />
        public SocketControllerMessageType Type { get; } = SocketControllerMessageType.RAW;

        /// <summary>
        /// Gets or sets a raw binary.
        /// </summary>
        /// <value>Byte array type of data.</value>
        public byte[] Raw { get; set; }
    }

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

    public enum SocketControllerMessageType
    {
        RAW,
        HANDSHAKE,
        HANDSHAKE_RESPONSE,
        SSL_CONNECTION_REQUEST,
    }
}
