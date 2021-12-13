using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public struct Handshake : IProtocol
    {
        public int ProtocolVersion { get; set; }

        public string ServerVersion { get; set; }

        public int ConnectionId { get; set; }

        public int CharacterSet { get; set; }

        public CapabilityFlag Capability { get; set; }

        public string? AuthPluginData { get; set; }

        public string? AuthPluginName { get; set; }

        public StatusFlag StatusFlag { get; set; }
    }
}
