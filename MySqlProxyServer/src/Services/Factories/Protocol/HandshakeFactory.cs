// Copyright (c) Min. All rights reserved.

using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class HandShakeFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(IPayloadData payloadData, ProxyState state)
        {
            if (payloadData.Payloads.Count() != 1)
            {
                throw new Exception("Payload count should be 1.");
            }

            var payload = payloadData.Payloads.First();

            using var stream = new MemoryStream(payload);
            using var reader = new BinaryReader(stream);

            var handshake = new Handshake();

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

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return handshake;
        }
    }
}
