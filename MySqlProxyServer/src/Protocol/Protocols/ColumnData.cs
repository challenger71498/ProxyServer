using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class ColumnData
    {
        public byte[] Catalog { get; set; }

        public byte[] Schema { get; set; }

        public byte[] Table { get; set; }

        public byte[] OrgTable { get; set; }

        public byte[] Name { get; set; }

        public byte[] OrgName { get; set; }

        public int NextLength { get; set; }

        public int CharacterSet { get; set; }

        public int ColumnLength { get; set; }

        public int Type { get; set; }

        public int Flags { get; set; }

        public int Decimals { get; set; }

        public byte[] ToBinary()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteLengthEncodedString(this.Catalog);
            writer.WriteLengthEncodedString(this.Schema);
            writer.WriteLengthEncodedString(this.Table);
            writer.WriteLengthEncodedString(this.OrgTable);
            writer.WriteLengthEncodedString(this.Name);
            writer.WriteLengthEncodedString(this.OrgName);

            writer.WriteLengthEncodedInt(0x0c);

            writer.WriteFixedInt(this.CharacterSet, 2);
            writer.WriteFixedInt(this.ColumnLength, 4);
            writer.WriteFixedInt(this.Type, 1);
            writer.WriteFixedInt(this.Flags, 2);
            writer.WriteFixedInt(this.Decimals, 1);

            writer.WriteFixedInt(0x00, 2);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }
    }
}