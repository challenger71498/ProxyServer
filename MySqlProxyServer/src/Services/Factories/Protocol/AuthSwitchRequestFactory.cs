using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class AuthSwitchRequestFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(IPayloadData payloadData, ProxyState state)
        {
            if (payloadData.Payloads.Count() != 1)
            {
                throw new Exception("Payload count should be 1.");
            }

            var payload = payloadData.Payloads.First();

            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            var flag = reader.ReadFixedInt(1);

            if (flag != 0xfe)
            {
                throw new Exception("Flag is not 0xFE.");
            }

            var request = new AuthSwitchRequest
            {
                AuthPluginName = reader.ReadNulTerminatedString(),
                AuthPluginData = reader.ReadRestOfPacketString(),
            };

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return request;
        }
    }
}