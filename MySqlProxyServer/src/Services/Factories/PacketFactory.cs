using System;
using System.IO;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer.Protocol
{
    public class PacketFactory
    {
        public IPacket Create(byte[] binary)
        {
            var packet = new PacketAdapter(binary);
            return packet;
        }

        public bool IsPacket(byte[] binary)
        {
            using var stream = new MemoryStream(binary);
            using var reader = new BinaryReader(stream);

            try
            {
                var length = reader.ReadFixedInt(3);

                return length == binary.Length - 4;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading binary. {e.Source} {e.Message}");
                Console.WriteLine(e.StackTrace);

                return false;
            }
        }
    }
}
