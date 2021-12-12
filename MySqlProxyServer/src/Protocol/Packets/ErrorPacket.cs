// // Copyright (c) Min. All rights reserved.

// using System.IO;

// namespace Min.MySqlProxyServer.Protocol
// {
//     public class ErrorPacket : BaseProtocol
//     {
//         public ErrorPacket(byte[] binary, CapabilityFlag? capability)
//             : base(binary)
//         {
//             this.capability = capability;
//         }

//         private CapabilityFlag? capability;

//         public int ErrorCode { get; private set; }

//         public string ErrorMessage { get; private set; }

//         public string? SqlState { get; private set; }

//         protected override void TryRead(BinaryReader reader)
//         {
//             reader.ReadByte(); // ERR packet header

//             this.ErrorCode = reader.ReadFixedInt(2);

//             if (this.capability?.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41) ?? false)
//             {
//                 reader.ReadByte(); // # marker of the SQL state

//                 this.SqlState = reader.ReadFixedString(5);
//             }

//             this.ErrorMessage = reader.ReadRestOfPacketString();
//         }
//     }
// }
