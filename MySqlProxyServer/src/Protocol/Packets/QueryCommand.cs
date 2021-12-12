namespace Min.MySqlProxyServer.Protocol
{
    public struct QueryCommand : IProtocol
    {
        public string Query { get; set; }
    }
}