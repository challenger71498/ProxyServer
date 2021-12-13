// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class QueryCommandFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(BinaryReader reader)
        {
            var queryCommand = default(QueryCommand);

            var code = reader.ReadByte();

            if (code != 0x03)
            {
                throw new Exception("Command code is not 0x03.");
            }

            queryCommand.Query = reader.ReadRestOfPacketString();

            return queryCommand;
        }
    }
}
