// // Copyright (c) Min. All rights reserved.

// using System;
// using System.IO;
// using Min.MySqlProxyServer.Protocol;

// namespace Min.MySqlProxyServer.Sockets
// {
//     public class ClientConnection : MySqlConnection
//     {
//         public ClientConnection(SocketConnection connection, CapabilityFlag capability)
//             : base(connection, capability)
//         {

//         }

//         protected override BaseProtocol GetProtocol(BinaryReader reader)
//         {
//             return this.Phase switch
//             {
//                 MySqlPhase.CONNECTION_PHASE => this.GetConnectionPhaseProtocol(reader),
//                 MySqlPhase.COMMAND_PHASE => this.GetCommandPhaseProtocol(reader),
//                 _ => throw new Exception("Invalid phase.")
//             };
//         }

//         private BaseProtocol GetConnectionPhaseProtocol(BinaryReader reader)
//         {

//         }

//         private BaseProtocol GetCommandPhaseProtocol(BinaryReader reader)
//         {

//         }
//     }
// }
