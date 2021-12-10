using System.IO;

namespace MySql.ProxyServer.Protocol
{
    public class PacketAdapter : IPacket
    {
        public int PayloadLength { get; private set; }
        public int SequenceId { get; private set; }
        public byte[] Payload { get; private set; }

        public PacketAdapter(byte[] binary)
        {
            using var stream = new MemoryStream(binary);
            using var reader = new BinaryReader(stream);

            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            this.PayloadLength = reader.ReadFixedInt(3);
            this.SequenceId = reader.ReadFixedInt(1);

            if (this.PayloadLength == 0) return;

            this.Payload = new byte[this.PayloadLength];
            reader.Read(this.Payload);
        }
    }
}
