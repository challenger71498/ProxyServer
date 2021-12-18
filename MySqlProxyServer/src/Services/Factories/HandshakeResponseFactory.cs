// Copyright (c) Min. All rights reserved.

using System;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class HandShakeResponseFactory : BaseProtocolFactory
    {
        protected override IProtocol Read(BinaryReader reader)
        {
            var handshakeResponse = new HandshakeResponse();

            handshakeResponse.Capability = (CapabilityFlag)reader.ReadFixedInt(4);
            handshakeResponse.MaxPacketSize = reader.ReadFixedInt(4);
            handshakeResponse.CharacterSet = reader.ReadFixedInt(1);

            reader.ReadBytes(23); // filler

            handshakeResponse.Username = reader.ReadNulTerminatedString();

            if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA))
            {
                handshakeResponse.AuthResponseLength = reader.ReadLengthEncodedInt();
                handshakeResponse.AuthResponse = reader.ReadNulTerminatedString();
            }
            else if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                var authResponseLength = reader.ReadFixedInt(1);
                handshakeResponse.AuthResponse = reader.ReadNulTerminatedString();
            }
            else
            {
                handshakeResponse.AuthResponse = reader.ReadNulTerminatedString();
            }

            if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_WITH_DB))
            {
                handshakeResponse.Database = reader.ReadNulTerminatedString();
            }

            if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH))
            {
                handshakeResponse.AuthPluginName = reader.ReadNulTerminatedString();
            }

            if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_CONNECT_ATTRS))
            {
                reader.ReadLengthEncodedInt(); // client connect attributes length

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var key = reader.ReadLengthEncodedString();
                    var value = reader.ReadLengthEncodedString();

                    handshakeResponse.Attributes.Add(key, value);
                }
            }

            return handshakeResponse;
        }
    }
}
