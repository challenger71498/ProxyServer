using System.IO;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class AuthSwitchRequestFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(BinaryReader reader)
        {
            var flag = reader.ReadFixedInt(1);

            if (flag != 0xfe)
            {
                throw new System.Exception("Flag is not 0xFE.");
            }

            var request = new AuthSwitchRequest
            {
                AuthPluginName = reader.ReadNulTerminatedString(),
                AuthPluginData = reader.ReadRestOfPacketString(),
            };

            return request;
        }
    }
}