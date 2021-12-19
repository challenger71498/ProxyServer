using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class AuthSwitchRequestMessage : IProtocolMessage
    {
        public AuthSwitchRequestMessage(AuthSwitchRequest authSwitchRequest)
        {
            this.Protocol = authSwitchRequest;
        }

        public IProtocol Protocol { get; }
    }
}