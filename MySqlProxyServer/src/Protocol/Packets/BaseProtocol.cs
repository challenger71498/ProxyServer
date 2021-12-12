// Copyright (c) Min. All rights reserved.

using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public abstract class BaseProtocol
    {
        protected BaseProtocol()
        {
        }

        protected abstract bool TryRead(byte[] data);
    }
}
