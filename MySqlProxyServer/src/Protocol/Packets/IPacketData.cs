using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public interface IPacketData : IData
    {
        IEnumerable<IPacket> Packets { get; }

        IBinaryData ToBinary();
    }

    public class PacketData : IPacketData
    {
        public PacketData(IEnumerable<IPacket> packets)
        {
            this.Packets = packets;
        }

        public IEnumerable<IPacket> Packets { get; }

        public IBinaryData ToBinary()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            foreach (var packet in this.Packets)
            {
                writer.Write(packet.ToBinary().Raw);
            }

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new BinaryData(binary);
        }
    }
}