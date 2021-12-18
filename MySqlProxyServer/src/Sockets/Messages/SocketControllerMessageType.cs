// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Sockets
{
    public enum SocketControllerMessageType
    {
        RAW,
        HANDSHAKE,
        HANDSHAKE_RESPONSE,
        SSL_CONNECTION_REQUEST,
    }
}
