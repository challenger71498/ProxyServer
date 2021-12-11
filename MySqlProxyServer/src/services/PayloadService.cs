using System;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class PayloadService
    {
        public PayloadService()
        {

        }

        public BaseProtocol GetProtocol(byte[] payload)
        {
            var header = payload[0];

            switch (header)
            {
                case 0x0a: // Handshake
                    return new Handshake(payload);
                default:
                    return null;
            }

            // switch (header)
            // {
            //     case 0x0a: // Handshake
            //         return;
            //     case 0x00: // OK_Packet
            //     case 0xfe:
            //         return;
            //     case 0xff: // ERR_Packet
            //         return;
            //     default:
            //         throw new Exception("Unsupported payload.");
            // }

            var handshake = new Handshake(payload);
        }
    }
}