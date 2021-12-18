// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Sockets
{
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
}
