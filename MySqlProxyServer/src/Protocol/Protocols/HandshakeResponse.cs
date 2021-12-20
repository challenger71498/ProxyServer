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

        public byte[] Username { get; set; }

        public byte[] AuthResponse { get; set; }

        public byte[] Database { get; set; }

        public byte[] AuthPluginName { get; set; }

        public int? AuthResponseLength { get; set; }

        public Dictionary<byte[], byte[]> Attributes { get; } = new Dictionary<byte[], byte[]>();

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = new List<IProtocolFactory>()
        {
            new AuthSwitchRequestFactory(),
        };

        public IEnumerable<byte[]> ToPayloads()
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
                writer.WriteFixedString(this.AuthResponse, (int)this.AuthResponseLength);
            }
            else if (this.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                if (this.AuthResponseLength == null)
                {
                    throw new Exception("CapabilityFlag CLIENT_SECURE_CONNECTION is set, but AuthResponseLength is null");
                }

                writer.WriteFixedInt((int)this.AuthResponseLength, 1);
                writer.WriteFixedString(this.AuthResponse, (int)this.AuthResponseLength);
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

            return new List<byte[]> { binary };
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
