// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer
{
    public class PacketService : IPacketService
    {
        private readonly PacketFactory packetFactory;

        public PacketService(PacketFactory packetFactory)
        {
            this.packetFactory = packetFactory;
        }

        public IObservable<IBinaryData> ToBinaryData(IObservable<IData> dataStream)
        {
            return dataStream.Select(data =>
            {
                if (data is IBinaryData binaryData)
                {
                    return binaryData;
                }

                if (data is not IPacket packet)
                {
                    throw new FormatException("Data format must be IPacket or IBinaryData.");
                }

                return packet.ToBinary();
            });
        }

        public IObservable<IData> FromBinaryData(IObservable<IBinaryData> binaryStream)
        {
            return binaryStream.Select<IBinaryData, IData>(binary =>
            {
                if (!this.packetFactory.IsPacket(binary.Raw))
                {
                    return binary;
                }

                var packet = this.packetFactory.Create(binary.Raw);
                return packet;
            });
        }
    }
}
