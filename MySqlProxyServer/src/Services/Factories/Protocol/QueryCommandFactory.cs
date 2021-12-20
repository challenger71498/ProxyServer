// Copyright (c) Min. All rights reserved.

using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class QueryCommandFactory : BaseProtocolFactory
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

            var queryCommand = new QueryCommand();

            var code = reader.ReadByte();

            if (code != 0x03)
            {
                throw new Exception("Command code is not 0x03.");
            }

            queryCommand.Query = reader.ReadRestOfPacketString();

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return queryCommand;
        }
    }
}
