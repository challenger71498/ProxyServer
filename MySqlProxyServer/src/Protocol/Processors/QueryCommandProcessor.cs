// Copyright (c) Min. All rights reserved.

using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class QueryCommandProcessor : IProtocolProcessor
    {
        public bool TryProcess(IProtocol protocol, out PayloadInfo info)
        {
            if (protocol.GetType() != typeof(QueryCommand))
            {
                info = default;
                return false;
            }

            var queryCommand = (QueryCommand)protocol;

            // TODO: Implement logging

            System.Console.WriteLine($"QUERY RECEIVED: {queryCommand.Query}");

            if (!queryCommand.Query.Contains("CHEQUER"))
            {
                info = default;
                return false;
            }

            System.Console.WriteLine("CHEQUER IS NOT ALLOWED!");

            var errorProtocol = new ErrorProtocol()
            {
                ErrorCode = 1045,
                ErrorMessage = "No permission to access the CHEQUER",
                SqlState = "28000",
            };

            var payloadInfo = new PayloadInfo
            {
                Payload = errorProtocol.ToPayload(),
                Loopback = true,
            };

            info = payloadInfo;
            return true;
        }
    }
}
