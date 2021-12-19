using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class AuthSwitchResponseMessage : IProtocolMessage
    {
        public AuthSwitchResponseMessage(AuthSwitchResponse authSwitchResponse)
        {
            this.Protocol = authSwitchResponse;
        }

        public SocketControllerMessageType Type => SocketControllerMessageType.AUTH_SWITCH_RESPONSE;

        public IProtocol Protocol { get; }
    }
}