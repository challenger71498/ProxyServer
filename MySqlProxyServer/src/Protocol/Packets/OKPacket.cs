namespace ProxyServer.Protocol
{
    using System.IO;
    using MySql.ProxyServer.Protocol;

    public class OKPacket
    {
        public int AffectedRows { get; private set; }
        public int LastInsertId { get; private set; }

        public StatusFlag Status { get; private set; }
        public int Warnings { get; private set; }



        public OKPacket(byte[] binary)
        {

        }

        public void Read(BinaryReader reader)
        {
            reader.ReadByte();

            AffectedRows = reader.ReadLengthEncodedInt();
            LastInsertId = reader.ReadLengthEncodedInt();
        }
    }
}