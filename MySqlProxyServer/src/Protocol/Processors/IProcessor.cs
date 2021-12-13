// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public interface IProtocolProcessor
    {
        bool TryProcess(IProtocol protocol, out PayloadInfo info);
    }
}
