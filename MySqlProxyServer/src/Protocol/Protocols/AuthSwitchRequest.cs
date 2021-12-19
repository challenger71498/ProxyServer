namespace Min.MySqlProxyServer.Protocol
{
    public class AuthSwitchRequest : IProtocol
    {
        public string AuthPluginName { get; set; }

        public string AuthPluginData { get; set; }
    }
}