using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public interface IProtocolMessage : ISocketControllerMessage
    {
        IProtocol Protocol { get; }
    }
}