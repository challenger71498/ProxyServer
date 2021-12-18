// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{

    public class MessageReceiver
    {
        private IPacketService packetService;

        private SequenceIdCounter counter;

        public MessageReceiver(SequenceIdCounter counter, IPacketService packetService)
        {
            this.counter = counter;
            this.packetService = packetService;
        }

        public IObservable<byte[]> GetDataStream(IObservable<ISocketControllerMessage> messageStream)
        {
            var dataStream = messageStream
                .Select(this.PipeMessage)
                .Select(this.PipeProtocol)
                .Let((payload) => this.packetService.PipePayload(this.counter.GetId, payload))
                .Select(this.PipePacket)
                .Catch((Exception e) =>
                {
                    throw e;
                })
                .Repeat();

            return dataStream;
        }

        private IProtocol PipeMessage(ISocketControllerMessage message);
        private byte[] PipeProtocol(IProtocol protocol);
        private IPacket[] PipePayload(byte[] payload);
        private byte[] PipePacket(IPacket packet);
    }
}