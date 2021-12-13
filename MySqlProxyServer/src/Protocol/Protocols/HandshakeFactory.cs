// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class HandShakeFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(BinaryReader reader)
        {
            /* TODO: Refactor method. */

            var handshake = default(Handshake);

            var authPluginDataAdapter = default(AuthPluginDataAdapter);
            var capabilityFlagAdapter = default(CapabilityFlagAdapter);

            handshake.ProtocolVersion = reader.ReadFixedInt(1);
            handshake.ServerVersion = reader.ReadNulTerminatedString();
            handshake.ConnectionId = reader.ReadFixedInt(4);

            authPluginDataAdapter.primary = reader.ReadFixedString(8);

            reader.ReadByte();  // filler

            capabilityFlagAdapter.lower = reader.ReadFixedInt(2);

            handshake.CharacterSet = reader.ReadFixedInt(1);

            var statusFlagInt = reader.ReadFixedInt(2);
            handshake.StatusFlag = (StatusFlag)statusFlagInt;

            capabilityFlagAdapter.upper = reader.ReadFixedInt(2);

            var authPluginDataLength = reader.ReadFixedInt(1);

            reader.ReadFixedString(10);   // reserved

            if (handshake.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                var authSecondaryLength = Math.Max(13, authPluginDataLength - 8);
                authPluginDataAdapter.secondary = reader.ReadFixedString(authSecondaryLength);
            }

            if (handshake.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH))
            {
                handshake.AuthPluginName = reader.ReadNulTerminatedString();
            }

            handshake.Capability = capabilityFlagAdapter.Value;
            handshake.AuthPluginData = authPluginDataAdapter.Value;

            return handshake;
        }
    }
}
