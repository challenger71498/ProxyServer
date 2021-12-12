namespace Min.MySqlProxyServer.Protocol
{
    public struct AuthPluginDataAdapter
    {
        // fields
        public string? primary;
        public string? secondary;

        // properties
        public string? Value
        {
            get
            {
                if (this.primary == null || this.secondary == null)
                {
                    return null;
                }

                return this.primary + this.secondary;
            }
        }
    }
}
