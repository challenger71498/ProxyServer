using System;
using System.Reactive.Linq;

namespace Min.MySqlProxyServer.Sockets
{
    public class MessageSender
    {
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;
        private readonly IProtocolService protocolService;
        private readonly IMessageService messageService;

        public MessageSender(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService,
            IMessageService messageService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
            this.messageService = messageService;
        }

        public IObservable<ISocketControllerMessage> GetMessageStream(IObservable<byte[]> dataStream)
        {
            var messageStream = dataStream
                .Select(binary => new BinaryData(binary))
                .Let(this.packetService.FromBinaryData)
                .Let(this.payloadService.FromPacket)
                .Let(this.protocolService.FromPayload)
                .Let(this.messageService.FromProtocol);

            return messageStream;
        }
    }
}