using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class HandshakeResponse : BaseProtocol
    {
        public HandshakeResponse(byte[] binary)
            : base(binary)
        {
        }

        public CapabilityFlag Capability { get; set; }

        public int MaxPacketSize { get; set; }

        public int CharacterSet { get; set; }

        public string Username { get; set; }

        public string AuthResponse { get; set; }

        public string Database { get; set; }

        public string AuthPluginName { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public byte[] ToBinary()
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
                writer.WriteFixedString(this.AuthResponse, this.AuthResponse.Length);
            }
            else if (this.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                writer.WriteFixedString(this.AuthResponse, 1);
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
                writer.WriteLengthEncodedInt(this.Attributes.Count);

                foreach (var pair in this.Attributes)
                {
                    writer.WriteLengthEncodedString(pair.Key);
                    writer.WriteLengthEncodedString(pair.Value);
                }
            }

            var binary = new byte[stream.Length];
            stream.GetBuffer().CopyTo(binary, 0);

            return binary;
        }

        protected override void Read(BinaryReader reader)
        {
            this.Capability = (CapabilityFlag)reader.ReadFixedInt(4);
            this.MaxPacketSize = reader.ReadFixedInt(4);
            this.CharacterSet = reader.ReadFixedInt(1);

            reader.ReadBytes(23); // filler

            this.Username = reader.ReadNulTerminatedString();

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA))
            {
                var authResponseLength = reader.ReadLengthEncodedInt();
                this.AuthResponse = reader.ReadFixedString(authResponseLength);
            }
            else if (this.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                var authResponseLength = reader.ReadFixedInt(1);
                this.AuthResponse = reader.ReadFixedString(authResponseLength);
            }
            else
            {
                this.AuthResponse = reader.ReadNulTerminatedString();
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_WITH_DB))
            {
                this.Database = reader.ReadNulTerminatedString();
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH))
            {
                this.AuthPluginName = reader.ReadNulTerminatedString();
            }

            if (this.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_ATTRS))
            {
                var keyValueLength = reader.ReadLengthEncodedInt();

                for (var i = 0; i < keyValueLength; ++i)
                {
                    var key = reader.ReadLengthEncodedString();
                    var value = reader.ReadLengthEncodedString();

                    this.Attributes.Add(key, value);
                }
            }
        }
    }
}