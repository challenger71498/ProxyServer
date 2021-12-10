namespace MySql.ProxyServer.Protocol
{
    public class AuthPluginDataAdapter
    {

        // fields
        private readonly string primary;
        private readonly string secondary;

        // properties
        public string Data => this.primary + this.secondary;

        public AuthPluginDataAdapter(string primary, string secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }
    }
}