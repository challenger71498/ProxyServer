using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public interface IProtocolFactory
    {
        bool TryCreate(byte[] data, out IProtocol protocol);
    }
}