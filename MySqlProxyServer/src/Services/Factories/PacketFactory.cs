using System;
using System.Collections.Generic;
using System.IO;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer.Protocol
{
    public class PacketFactory
    {
        public IEnumerable<IPacket>? TryCreate(byte[] binary)
        {
            var packets = new List<IPacket>();

            using var stream = new MemoryStream(binary);
            using var reader = new BinaryReader(stream);

            try
            {
                while (stream.Position < stream.Length)
                {
                    var lengthBuffer = reader.ReadBytes(3);

                    stream.Seek(-3, SeekOrigin.Current);
                    var length = reader.ReadFixedInt(3);

                    if (length != 0)
                    {
                        var sequenceIdBuffer = reader.ReadBytes(1);
                        var payloadBuffer = reader.ReadBytes(length);

                        var buffer = new byte[3 + 1 + length];

                        lengthBuffer.CopyTo(buffer, 0);
                        sequenceIdBuffer.CopyTo(buffer, 3);
                        payloadBuffer.CopyTo(buffer, 3 + 1);

                        // Console.WriteLine(Convert.ToHexString(buffer));

                        var packet = new PacketAdapter(buffer);
                        packets.Add(packet);
                    }

                    if (stream.Position == stream.Length)
                    {
                        return packets;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading binary. {e.Source} {e.Message}");
                Console.WriteLine(e.StackTrace);

                return null;
            }
        }
    }
}
