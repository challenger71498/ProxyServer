// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public abstract class BaseProtocolFactory : IProtocolFactory
    {
        public bool TryCreate(byte[] data, out IProtocol protocol)
        {
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            try
            {
                protocol = Read(reader);

                if (stream.Position != stream.Length)
                {
                    Console.WriteLine($"Reader did not reach EOF. {stream.Position} {stream.Length}");
                    throw new Exception("Reader did not reach EOF.");
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error! {e.GetType()} {e.Message}");
                Console.WriteLine(e.StackTrace);

                protocol = default(HandshakeResponse);
                return false;
            }
        }

        protected abstract IProtocol Read(BinaryReader reader);
    }
}