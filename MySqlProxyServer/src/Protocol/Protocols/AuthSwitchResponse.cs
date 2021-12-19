using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class AuthSwitchResponse : IWritableProtocol
    {
        public string AuthPluginResponse { get; set; }

        public byte[] ToPayload()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedString(this.AuthPluginResponse, this.AuthPluginResponse.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }
    }
}