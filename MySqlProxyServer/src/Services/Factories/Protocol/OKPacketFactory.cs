using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class OKPacketFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(IPayloadData payloadData, ProxyState state)
        {
            if (state.Capability is not CapabilityFlag capability)
            {
                throw new Exception("Capability is null.");
            }

            if (payloadData.Payloads.Count() != 1)
            {
                throw new Exception("Payload count should be 1.");
            }

            var payload = payloadData.Payloads.First();

            Console.WriteLine(Convert.ToHexString(payload));

            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            var packet = new OKPacket(capability)
            {
                Header = reader.ReadByte(),

                AffectedRows = reader.ReadLengthEncodedInt(),
                LastInsertId = reader.ReadLengthEncodedInt(),
            };

            ReadStatus(reader, packet, capability);

            if (stream.Position < stream.Length)
            {
                ReadInfo(reader, packet, capability);
            }

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return packet;
        }

        private static void ReadStatus(BinaryReader reader, OKPacket packet, CapabilityFlag capability)
        {
            if (capability.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41))
            {
                var statusNumber = reader.ReadFixedInt(2);
                packet.Status = (StatusFlag)statusNumber;

                packet.Warnings = reader.ReadFixedInt(2);
            }
            else if (capability.HasFlag(CapabilityFlag.CLIENT_TRANSACTIONS))
            {
                var statusNumber = reader.ReadFixedInt(2);
                packet.Status = (StatusFlag)statusNumber;
            }
        }

        private static void ReadInfo(BinaryReader reader, OKPacket packet, CapabilityFlag capability)
        {
            if (capability.HasFlag(CapabilityFlag.CLIENT_SESSION_TRACK))
            {
                packet.Info = reader.ReadLengthEncodedString();

                if (packet.Status?.HasFlag(StatusFlag.SERVER_SESSION_STATE_CHANGED) ?? false)
                {
                    packet.SessionStateInfo = reader.ReadLengthEncodedString();
                }
            }
            else
            {
                packet.Info = reader.ReadRestOfPacketString();
            }
        }
    }
}