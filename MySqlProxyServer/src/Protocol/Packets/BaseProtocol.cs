// Copyright (c) Min. All rights reserved.

using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public abstract class BaseProtocol
    {
        public BaseProtocol(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);

            this.Read(reader);
        }

        protected abstract void Read(BinaryReader reader);
    }
}