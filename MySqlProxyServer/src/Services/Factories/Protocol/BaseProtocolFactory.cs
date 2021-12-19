// Copyright (c) Min. All rights reserved.

using System;
using System.IO;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public abstract class BaseProtocolFactory : IProtocolFactory
    {
        public bool TryCreate(byte[] data, out IProtocol protocol)
        {
            var stream = new MemoryStream(data);
            var reader = new BinaryReader(stream);

            try
            {
                Console.WriteLine(Convert.ToHexString(data));
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(data));

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
                // Console.WriteLine($"Error! {e.GetType()} {e.Message}");
                // Console.WriteLine(e.StackTrace);

                // TODO: Handle exception properly.
                protocol = null;
                return false;
            }
        }

        protected abstract IProtocol Read(BinaryReader reader);
    }
}