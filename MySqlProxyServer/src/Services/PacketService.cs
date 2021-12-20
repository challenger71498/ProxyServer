// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                if (data is not IPacketData packetData)
                {
                    throw new FormatException($"Data format {data.GetType()} must be IPacket or IBinaryData.");
                }

                return packetData.ToBinary();
            });
        }

        public IObservable<IData> FromBinaryData(IObservable<IBinaryData> binaryStream)
        {
            return binaryStream.Select<IBinaryData, IData>(binary =>
            {
                var packets = this.packetFactory.TryCreate(binary.Raw);

                if (packets == null)
                {
                    return binary;
                }

                return new PacketData(packets);
            });
        }
    }
}
