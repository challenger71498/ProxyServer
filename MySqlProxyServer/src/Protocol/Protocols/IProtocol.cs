using System.Collections.Generic;

namespace Min.MySqlProxyServer.Protocol
{
    public interface IProtocol : IData
    {
        IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; }
    }
}
