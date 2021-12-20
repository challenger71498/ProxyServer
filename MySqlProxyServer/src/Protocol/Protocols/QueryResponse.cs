using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Min.MySqlProxyServer.Protocol
{

    public class QueryResponse : IWritableProtocol
    {
        public int ColumnCount { get; set; }

        public IEnumerable<ColumnData> Columns { get; set; }

        public IEnumerable<byte[]?[]> Rows { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = null;

        public OKPacket OKPacket { get; set; }

        public IEnumerable<byte[]> ToPayloads()
        {
            try
            {
                var results = new List<byte[]>();

                using var stream = new MemoryStream();
                using var writer = new BinaryWriter(stream);

                writer.WriteLengthEncodedInt(this.ColumnCount);

                var buffer = stream.GetBuffer();
                var binary = buffer[..(int)stream.Length];

                results.Add(binary);

                foreach (var column in this.Columns)
                {
                    results.Add(column.ToBinary());
                }

                results.AddRange(this.GetBinaryRows());

                var packetBinary = this.OKPacket.ToPayloads().ElementAt(0);

                results.Add(packetBinary);

                return results;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
                throw e;
            }
        }

        private IEnumerable<byte[]> GetBinaryRows()
        {
            var results = new List<byte[]>();

            foreach (var row in this.Rows)
            {
                using var stream = new MemoryStream();
                using var writer = new BinaryWriter(stream);

                foreach (var element in row)
                {
                    if (element == null)
                    {
                        writer.WriteFixedInt(0xfb, 1);
                    }
                    else
                    {
                        writer.WriteLengthEncodedString(element);
                    }
                }

                var buffer = stream.GetBuffer();
                var binary = buffer[..(int)stream.Length];

                results.Add(binary);
            }

            return results;
        }
    }
}