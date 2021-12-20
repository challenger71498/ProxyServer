using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class ErrorProtocolFactory : BaseProtocolFactory
    {
        private readonly CapabilityFlag? capability;

        public ErrorProtocolFactory(CapabilityFlag? capability)
        {
            this.capability = capability;
        }

        protected override IProtocol Read(IPayloadData payloadData, ProxyState state)
        {
            if (payloadData.Payloads.Count() != 1)
            {
                throw new Exception("Payload count should be 1.");
            }

            var payload = payloadData.Payloads.First();

            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            var errorProtocol = new ErrorProtocol();

            reader.ReadByte(); // ERR packet header

            errorProtocol.ErrorCode = reader.ReadFixedInt(2);

            if (this.capability?.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41) ?? false)
            {
                reader.ReadByte(); // # marker of the SQL state

                errorProtocol.SqlState = reader.ReadFixedString(5);
            }

            errorProtocol.ErrorMessage = reader.ReadRestOfPacketString();

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return errorProtocol;
        }
    }
}
