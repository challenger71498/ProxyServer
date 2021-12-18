// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{

    public class MessageReceiver
    {
        private IPacketService packetService;

        public MessageReceiver(IPacketService packetService)
        {
            this.packetService = packetService;
        }

        public IObservable<byte[]> GetDataStream(IObservable<ISocketControllerMessage> messageStream)
        {
            var dataStream = messageStream
                .Select(this.PipeMessage)
                .Select(this.PipeProtocol)
                .Let((payload) => this.packetService.PipePayload(1, payload))
                .Select(this.PipePacket)
                .sel;
        }

        private IProtocol PipeMessage(ISocketControllerMessage message);
        private byte[] PipeProtocol(IProtocol protocol);
        private IPacket[] PipePayload(byte[] payload);
        private byte[][] PipePacket(IPacket[] packets);
    }
}