using System.Collections.Generic;

namespace Min.MySqlProxyServer.Protocol
{
    public interface IWritableProtocol : IProtocol
    {
        IEnumerable<byte[]> ToPayloads();
    }
}