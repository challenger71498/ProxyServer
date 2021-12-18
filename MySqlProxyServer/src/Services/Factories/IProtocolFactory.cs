namespace Min.MySqlProxyServer.Protocol
{
    public interface IProtocolFactory
    {
        bool TryCreate(byte[] data, out IProtocol protocol);
    }
}