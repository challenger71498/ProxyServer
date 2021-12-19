using System.IO;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class AuthSwitchResponseFactory : BaseProtocolFactory
    {
        /// <inheritdoc/>
        protected override IProtocol Read(BinaryReader reader)
        {
            var response = new AuthSwitchResponse
            {
                AuthPluginResponse = reader.ReadRestOfPacketString(),
            };

            return response;
        }
    }
}