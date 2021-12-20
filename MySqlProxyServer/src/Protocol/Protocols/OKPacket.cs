using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class OKPacket : IWritableProtocol
    {
        private CapabilityFlag capability;

        public OKPacket(CapabilityFlag capability)
        {
            this.capability = capability;
        }

        public int Header { get; set; }

        public int AffectedRows { get; set; }

        public int LastInsertId { get; set; }

        public byte[] Info { get; set; }

        public StatusFlag? Status { get; set; }

        public int? Warnings { get; set; }

        public byte[]? SessionStateInfo { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = null;

        public IEnumerable<byte[]> ToPayloads()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(this.Header, 1);

            writer.WriteLengthEncodedInt(this.AffectedRows);
            writer.WriteLengthEncodedInt(this.LastInsertId);

            this.WriteStatus(writer);
            this.WriteInfo(writer);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new List<byte[]> { binary };
        }

        private void WriteStatus(BinaryWriter writer)
        {
            if (this.capability.HasFlag(CapabilityFlag.CLIENT_PROTOCOL_41))
            {
                writer.WriteFixedInt((int)this.Status, 2);
                writer.WriteFixedInt((int)this.Warnings, 2);
            }
            else if (this.capability.HasFlag(CapabilityFlag.CLIENT_TRANSACTIONS))
            {
                writer.WriteFixedInt((int)this.Status, 2);
            }
        }

        private void WriteInfo(BinaryWriter writer)
        {
            if (this.Info == null)
            {
                return;
            }

            if (this.capability.HasFlag(CapabilityFlag.CLIENT_SESSION_TRACK))
            {
                writer.WriteLengthEncodedString(this.Info);

                if (this.Status?.HasFlag(StatusFlag.SERVER_SESSION_STATE_CHANGED) ?? false)
                {
                    writer.WriteLengthEncodedString(this.SessionStateInfo);
                }
            }
            else
            {
                if (this.Info != null)
                {
                    writer.WriteFixedString(this.Info, this.Info.Length);
                }
            }

        }
    }
}