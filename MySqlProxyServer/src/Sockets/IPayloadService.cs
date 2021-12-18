// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    // TODO: Move away dummy IPayloadService interface.
    public interface IPayloadService
    {
        IProtocol? TryGetProtocol(byte[] payload);
    }
}
