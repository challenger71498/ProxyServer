// Copyright (c) Min. All rights reserved.

using System.IO;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class ErrorProtocolFactory : BaseProtocolFactory
    {
        private readonly CapabilityFlag? capability;

        public ErrorProtocolFactory(CapabilityFlag? capability)
        {
            this.capability = capability;
        }

        protected override IProtocol Read(BinaryReader reader)
        {
            var errorProtocol = default(ErrorProtocol);

            reader.ReadByte(); // ERR packet header

            errorProtocol.ErrorCode = reader.ReadFixedInt(2);

            if (this.capability?.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41) ?? false)
            {
                reader.ReadByte(); // # marker of the SQL state

                errorProtocol.SqlState = reader.ReadFixedString(5);
            }

            errorProtocol.ErrorMessage = reader.ReadRestOfPacketString();

            return errorProtocol;
        }
    }
}
