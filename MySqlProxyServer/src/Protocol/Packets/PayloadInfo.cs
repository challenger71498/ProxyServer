// Copyright (c) Min. All rights reserved.

namespace Min.MySqlProxyServer.Protocol
{
    public struct PayloadInfo
    {
        public byte[] Payload { get; set; }

        public bool Loopback { get; set; }
    }
}
