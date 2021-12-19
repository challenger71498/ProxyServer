using System;
using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class Handshake : IProtocol
    {
        public int ProtocolVersion { get; set; }

        public byte[] ServerVersion { get; set; }

        public int ConnectionId { get; set; }

        public int CharacterSet { get; set; }

        public CapabilityFlag Capability { get; set; }

        public byte[]? AuthPluginData { get; set; }

        public byte[]? AuthPluginName { get; set; }

        public StatusFlag StatusFlag { get; set; }

        public List<Type> NextProtocolFactories => new()
        {
            typeof(HandshakeResponse),
        };

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = new List<IProtocolFactory>
        {
            new HandShakeResponseFactory(),
        };
    }
}
