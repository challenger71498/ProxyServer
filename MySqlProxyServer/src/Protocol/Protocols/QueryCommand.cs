using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class QueryCommand : IWritableProtocol
    {
        public byte[] Query { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = new List<IProtocolFactory>
        {
            new QueryResponseFactory(),
        };

        public IEnumerable<byte[]> ToPayloads()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(0x03, 1);

            writer.WriteFixedString(this.Query, this.Query.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new List<byte[]> { binary };
        }
    }
}
