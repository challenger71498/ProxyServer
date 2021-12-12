// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Protocol
{
    public interface IHandShake
    {
        int ProtocolVersion { get; set; }

        string ServerVersion { get; set; }

        int ConnectionId { get; set; }

        int CharacterSet { get; set; }

        CapabilityFlag Capability { get; set; }

        StatusFlag StatusFlag { get; set; }

        string? AuthPluginData { get; set; }

        string? AuthPluginName { get; set; }
    }
}
