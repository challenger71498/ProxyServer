// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Sockets
{
    public interface ISocketControllerMessage : IData
    {
        /// <summary>
        /// Gets the type of the socket controller message.
        /// </summary>
        /// <value>An enum value.</value>
        SocketControllerMessageType Type { get; }
    }
}
