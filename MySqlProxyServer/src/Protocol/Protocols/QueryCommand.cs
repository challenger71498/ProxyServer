using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class QueryCommand : IProtocol
    {
        public byte[] Query { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = null;
    }
}
