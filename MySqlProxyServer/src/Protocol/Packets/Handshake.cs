using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class Handshake : BaseProtocol
    {
        private AuthPluginDataAdapter authPluginDataAdapter;
        private CapabilityFlagAdapter capabilityFlagAdapter;

        public int ProtocolVersion { get; private set; }

        public string ServerVersion { get; private set; }

        public int ConnectionId { get; private set; }

        public string AuthPluginData => authPluginDataAdapter.Data;

        public string AuthPluginName { get; private set; }

        public CapabilityFlag? Capability => capabilityFlagAdapter.Data;

        public int CharacterSet { get; private set; }

        public StatusFlag? StatusFlag { get; private set; }

        public Handshake(byte[] binary)
            : base(binary)
        {
        }

        protected override void Read(BinaryReader reader)
        {
            string authPrimary;
            string authSecondary;

            int capabilityLower;
            int capabilityUpper;

            this.ProtocolVersion = reader.ReadFixedInt(1);
            this.ServerVersion = reader.ReadNulTerminatedString();
            this.ConnectionId = reader.ReadFixedInt(4);

            authPrimary = reader.ReadFixedString(8);

            reader.ReadByte();  // filler

            capabilityLower = reader.ReadFixedInt(2);

            var characterSetInt = reader.ReadFixedInt(1);
            this.CharacterSet = characterSetInt;

            var statusFlagInt = reader.ReadFixedInt(2);
            this.StatusFlag = (StatusFlag)statusFlagInt;

            capabilityUpper = reader.ReadFixedInt(2);

            this.capabilityFlagAdapter = new CapabilityFlagAdapter(capabilityLower, capabilityUpper);

            var authPluginDataLength = reader.ReadFixedInt(1);

            reader.ReadFixedString(10);   // reserved

            if (this.capabilityFlagAdapter.Data.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                var authSecondaryLength = Math.Max(13, authPluginDataLength - 8);
                authSecondary = reader.ReadFixedString(authSecondaryLength);

                this.authPluginDataAdapter = new AuthPluginDataAdapter(authPrimary, authSecondary);
            }

            if (this.capabilityFlagAdapter.Data.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH))
            {
                this.AuthPluginName = reader.ReadNulTerminatedString();
            }
        }
    }
}
