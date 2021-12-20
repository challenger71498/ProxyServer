using System.Collections.Generic;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class ProtocolService
    {
        private IEnumerable<IProtocolFactory> defaultFactories;

        public ProtocolService(IEnumerable<IProtocolFactory> defaultFactories)
        {
            this.defaultFactories = defaultFactories;
        }

        public IEnumerable<IProtocolFactory> GetAvailableFactories(IData? data)
        {
            if (data == null)
            {
                return this.defaultFactories;
            }

            if (data is not IProtocol protocol)
            {
                return this.defaultFactories;
            }

            var availableFactories = protocol.NextAvailableProtocolFactories;

            return availableFactories ?? this.defaultFactories;
        }
    }
}
