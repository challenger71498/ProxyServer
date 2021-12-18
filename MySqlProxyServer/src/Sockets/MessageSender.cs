// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class MessageSender
    {
        private readonly IPacketService packetService;
        private readonly IProtocolFactory[] factories;

        public MessageSender(IPacketService packetService, params IProtocolFactory[] factories)
        {
            this.packetService = packetService;
            this.factories = factories;
        }

        public IObservable<ISocketControllerMessage> GetMessageStream(IObservable<byte[]> dataStream)
        {
            byte[]? captured = null;

            var messageStream = dataStream
                .Do(data => captured = data)
                .Select(this.PipeRawData)
                .Let(this.packetService.PipePacket)
                .Select(this.PipePayload)
                .Select(this.PipeProtocol)
                .Catch((MessageSenderException e) =>
                {
                    if (captured == null)
                    {
                        throw new NullReferenceException("Captured data is null. This should never be happened.");
                    }

                    var rawMessage = new RawDataMessage(captured);
                    return Observable.ToObservable(new[] { rawMessage });
                })
                .Catch((Exception e) =>
                {
                    throw e;
                })
                .Repeat();

            return messageStream;
        }

        private IPacket PipeRawData(byte[] data)
        {
            var packet = PacketFactory.TryCreatePacket(data);

            if (packet != null)
            {
                return packet;
            }

            throw new MessageSenderException("Failed to get packet. Streaming directly to the messenger...");
        }

        private IProtocol PipePayload(IPayloadData payloadData)
        {
            // Get protocol from packet.
            // Throw SocketControllerException if failed.
            foreach (var factory in this.factories)
            {
                if (factory.TryCreate(payloadData.Payload, out var protocol))
                {
                    return protocol;
                }
            }

            throw new MessageSenderException("Cannot create a protocol.");
        }

        private ISocketControllerMessage PipeProtocol(IProtocol protocolStream)
        {
            // Create message from the protocol.
            // Throw SocketControllerException if failed.
            foreach (var factory in this.factories)
            {
                factory.TryCreate()
            }

            return null;
        }

        // NOTE: Is it okay to use internal exception?
        private class MessageSenderException : Exception
        {
            public MessageSenderException(string message)
            : base(message)
            {
            }
        }
    }
}