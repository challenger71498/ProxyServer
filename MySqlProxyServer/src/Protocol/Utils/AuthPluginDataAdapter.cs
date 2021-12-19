namespace Min.MySqlProxyServer.Protocol
{
    public struct AuthPluginDataAdapter
    {
        // fields
        public byte[]? primary;
        public byte[]? secondary;

        // properties
        public byte[]? Value
        {
            get
            {
                if (this.primary == null || this.secondary == null)
                {
                    return null;
                }

                var concat = new byte[this.primary.Length + this.secondary.Length];
                this.primary.CopyTo(concat, 0);
                this.secondary.CopyTo(concat, this.primary.Length);

                return concat;
            }
        }
    }
}
