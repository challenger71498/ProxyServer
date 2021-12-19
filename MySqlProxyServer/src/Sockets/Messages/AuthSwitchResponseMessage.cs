using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class AuthSwitchResponseMessage : ISocketControllerMessage
    {
        public SocketControllerMessageType Type => SocketControllerMessageType.AUTH_SWITCH_RESPONSE;

        public AuthSwitchResponse Protocol { get; set; }
    }
}