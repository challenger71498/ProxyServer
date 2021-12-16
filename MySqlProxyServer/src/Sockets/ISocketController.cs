// Copyright (c) Min. All rights reserved.

using System;

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
        ISocketControllerMessageType Type { get; }
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
        public ISocketControllerMessageType Type { get; } = ISocketControllerMessageType.RAW;

        /// <summary>
        /// Gets or sets a raw binary.
        /// </summary>
        /// <value>Byte array type of data.</value>
        public byte[] Raw { get; set; }
    }

    public class HandshakeControllerMessage : ISocketControllerMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeControllerMessage"/> class.
        /// </summary>
        /// <param name="packets">Array of packets.</param>
        public HandshakeControllerMessage(byte[][] packets)
        {
            this.Packets = packets;
        }

        /// <inheritdoc/>
        public ISocketControllerMessageType Type { get; } = ISocketControllerMessageType.HANDSHAKE;

        /// <summary>
        /// Gets or sets a list of packet.
        /// </summary>
        /// <value>Array of packets.</value>
        public byte[][] Packets { get; set; }
    }

    public enum ISocketControllerMessageType
    {
        RAW,
        HANDSHAKE,
        HANDSHAKE_RESPONSE,
        SSL_CONNECTION_REQUEST,
    }
}
