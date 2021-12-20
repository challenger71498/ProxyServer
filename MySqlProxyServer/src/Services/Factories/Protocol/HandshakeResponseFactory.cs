// Copyright (c) Min. All rights reserved.

using System;
using System.IO;
using System.Linq;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class HandShakeResponseFactory : BaseProtocolFactory
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

            var handshakeResponse = new HandshakeResponse();

            handshakeResponse.Capability = (CapabilityFlag)reader.ReadFixedInt(4);
            handshakeResponse.MaxPacketSize = reader.ReadFixedInt(4);
            handshakeResponse.CharacterSet = reader.ReadFixedInt(1);

            reader.ReadBytes(23); // filler

            handshakeResponse.Username = reader.ReadNulTerminatedString();

            if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA))
            {
                var length = handshakeResponse.AuthResponseLength = reader.ReadLengthEncodedInt();
                handshakeResponse.AuthResponse = reader.ReadFixedString((int)length);
            }
            else if (handshakeResponse.Capability.HasFlag(CapabilityFlag.CLIENT_SECURE_CONNECTION))
            {
                var length = reader.ReadFixedInt(1);
                handshakeResponse.AuthResponse = reader.ReadFixedString((int)length);
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

            if (stream.Position != stream.Length)
            {
                throw new Exception($"Reader {this.GetType()} did not reach EOF. {stream.Position} {stream.Length}");
            }

            return handshakeResponse;
        }
    }
}
