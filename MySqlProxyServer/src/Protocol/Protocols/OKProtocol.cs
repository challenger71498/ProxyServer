// // Copyright (c) Min. All rights reserved.

// using System.IO;

// namespace Min.MySqlProxyServer.Protocol
// {
//     public class OKPacket : BaseProtocol
//     {
//         public OKPacket(byte[] binary, CapabilityFlag capability)
//             : base(binary)
//         {
//             this.capability = capability;
//         }

//         private CapabilityFlag capability;

//         public int AffectedRows { get; private set; }

//         public int LastInsertId { get; private set; }

//         public string Info { get; private set; }

//         public StatusFlag? Status { get; private set; }

//         public int? Warnings { get; private set; }

//         public string? SessionStateInfo { get; private set; }

//         protected override void TryRead(BinaryReader reader)
//         {
//             reader.ReadByte();  // skip packet header

//             this.AffectedRows = reader.ReadLengthEncodedInt();
//             this.LastInsertId = reader.ReadLengthEncodedInt();

//             this.ReadStatus(reader);
//             this.ReadInfo(reader);
//         }

//         private void ReadStatus(BinaryReader reader)
//         {
//             if (this.capability.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41))
//             {
//                 var statusNumber = reader.ReadFixedInt(2);
//                 this.Status = (StatusFlag)statusNumber;

//                 this.Warnings = reader.ReadFixedInt(2);
//             }
//             else if (this.capability.HasFlag(CapabilityFlag.CLIENT_TRANSACTIONS))
//             {
//                 var statusNumber = reader.ReadFixedInt(2);
//                 this.Status = (StatusFlag)statusNumber;
//             }
//         }

//         private void ReadInfo(BinaryReader reader)
//         {
//             if (this.capability.HasFlag(CapabilityFlag.CLIENT_SESSION_TRACK))
//             {
//                 this.Info = reader.ReadLengthEncodedString();

//                 if (this.Status?.HasFlag(StatusFlag.SERVER_SESSION_STATE_CHANGED) ?? false)
//                 {
//                     this.SessionStateInfo = reader.ReadLengthEncodedString();
//                 }
//             }
//             else
//             {
//                 this.Info = reader.ReadRestOfPacketString();
//             }
//         }
//     }
// }