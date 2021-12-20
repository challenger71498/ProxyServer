using System.Collections.Generic;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public interface IProtocolFactory
    {
        bool TryCreate(IPayloadData data, out IProtocol protocol, ProxyState state);
    }
}