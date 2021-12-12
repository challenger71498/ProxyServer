// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class QueryCommandFactory : IProtocolFactory
    {
        public bool TryCreate(byte[] data, out IProtocol protocol)
        {
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            try
            {
                protocol = Read(reader);
                return true;
            }
            catch (Exception)
            {
                protocol = default(QueryCommand);
                return false;
            }
        }

        private static QueryCommand Read(BinaryReader reader)
        {
            var queryCommand = default(QueryCommand);

            reader.ReadByte();  // COM_QUERY

            queryCommand.Query = reader.ReadRestOfPacketString();

            return queryCommand;
        }
    }
}
