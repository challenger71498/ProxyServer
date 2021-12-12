// namespace Min.MySqlProxyServer.Protocol
// {
//     public class PacketFactory
//     {
//         public static P? CreatePacket<P>(byte[] binary)
//             where P : BaseProtocol
//         {
//             if (typeof(P) == typeof(Handshake))
//             {

//             }
//         }

//         private static Handshake? CreateHandshake(byte[] binary)
//         {
//             if (false /* TODO: Create handshake vaildation */)
//             {
//                 return null;
//             }

//             var handShake = new Handshake(binary);
//         }
//     }
// }