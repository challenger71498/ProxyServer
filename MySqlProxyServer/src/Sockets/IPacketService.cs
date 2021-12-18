// Copyright (c) Min. All rights reserved.

using System;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    // TODO: Move away dummy IPacketService interface.
    public interface IPacketService
    {
        IObservable<IPayloadData> PipePacket(IObservable<IPacket> packetStream);

        IObservable<IPacket> PipePayload(int id, IObservable<byte[]> payload);
    }
}
