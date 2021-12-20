using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class AuthSwitchRequest : IWritableProtocol
    {
        public byte[] AuthPluginName { get; set; }

        public byte[] AuthPluginData { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = new List<IProtocolFactory>
        {
            new AuthSwitchResponseFactory(),
        };

        public IEnumerable<byte[]> ToPayloads()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(0xfe, 1);

            writer.WriteNulTerminatedString(this.AuthPluginName);
            writer.WriteFixedString(this.AuthPluginData, this.AuthPluginData.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new List<byte[]> { binary };
        }
    }
}