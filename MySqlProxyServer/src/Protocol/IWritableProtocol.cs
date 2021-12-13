namespace Min.MySqlProxyServer.Protocol
{
    public interface IWritableProtocol : IProtocol
    {
        byte[] ToBinary();
    }
}