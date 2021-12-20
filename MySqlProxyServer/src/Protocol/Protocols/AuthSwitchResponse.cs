using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class AuthSwitchResponse : IWritableProtocol
    {
        public byte[] AuthPluginResponse { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = null;

        public IEnumerable<byte[]> ToPayloads()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedString(this.AuthPluginResponse, this.AuthPluginResponse.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new List<byte[]> { binary };
        }
    }
}