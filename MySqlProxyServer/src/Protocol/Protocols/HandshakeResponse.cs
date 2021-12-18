using System;
using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class HandshakeResponse : IWritableProtocol
    {
        public CapabilityFlag Capability { get; set; }

        public int MaxPacketSize { get; set; }

        public int CharacterSet { get; set; }

        public string Username { get; set; }

        public string AuthResponse { get; set; }

        public string Database { get; set; }

        public string AuthPluginName { get; set; }

        public int? AuthResponseLength { get; set; }

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public byte[] ToPayload()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt((int)this.Capability, 4);
            writer.WriteFixedInt(this.MaxPacketSize, 4);
            writer.WriteFixedInt(this.CharacterSet, 1);

            writer.WriteFixedInt(0x00, 23); // filler

            writer.WriteNulTerminatedString(this.Username);

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA))
            {
                if (this.AuthResponseLength == null)
                {
                    throw new Exception("CapabilityFlag CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA is set, but AuthResponseLength is null");
                }

                writer.WriteLengthEncodedInt((int)this.AuthResponseLength);
                writer.WriteNulTerminatedString(this.AuthResponse);
            }
            else if (this.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                writer.WriteNulTerminatedString(this.AuthResponse);
            }
            else
            {
                writer.WriteNulTerminatedString(this.AuthResponse);
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_WITH_DB))
            {
                writer.WriteNulTerminatedString(this.Database);
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH))
            {
                writer.WriteNulTerminatedString(this.AuthPluginName);
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_ATTRS))
            {
                this.WriteAttribute(writer);
            }

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }

        private void WriteAttribute(BinaryWriter writer)
        {
            using var attributeStream = new MemoryStream();
            using var attributeWriter = new BinaryWriter(attributeStream);

            foreach (var pair in this.Attributes)
            {
                attributeWriter.WriteLengthEncodedString(pair.Key);
                attributeWriter.WriteLengthEncodedString(pair.Value);
            }

            var count = (int)attributeStream.Position;
            var streamBuffer = attributeStream.GetBuffer();

            var binary = streamBuffer[..count];

            writer.WriteLengthEncodedInt(count);
            writer.Write(binary);
        }
    }
}
