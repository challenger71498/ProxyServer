// using Min.MySqlProxyServer.Protocol;

// namespace Min.MySqlProxyServer
// {
//     public class HandshakeResponseProcessor : IProtocolProcessor
//     {
//         public bool TryProcess(IProtocol protocol, out PayloadInfo info)
//         {
//             if (protocol.GetType() != typeof(HandshakeResponse))
//             {
//                 info = default;
//                 return false;
//             }

//             var handshakeResponse = (HandshakeResponse)protocol;

//             System.Console.WriteLine($"AUTH_RESPONSE: {handshakeResponse.AuthResponse}");

//             handshakeResponse.Username = "root";

//             handshakeResponse.AuthResponse = "foobar";
//             handshakeResponse.AuthResponseLength = 7;

//             var payload = handshakeResponse.ToPayload();

//             info = new PayloadInfo { Payload = payload };
//             return true;
//         }
//     }
// }
