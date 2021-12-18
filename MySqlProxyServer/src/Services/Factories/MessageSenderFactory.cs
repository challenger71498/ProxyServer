using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class MessageSenderFactory
    {
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;
        private readonly IProtocolService protocolService;
        private readonly IMessageService messageService;

        public MessageSenderFactory(
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

        public MessageSender Create()
        {
            var sender = new MessageSender(this.packetService, this.payloadService, this.protocolService, this.messageService);
            return sender;
        }
    }

    public class MessageReceiverFactory
    {
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;
        private readonly IProtocolService protocolService;
        private readonly IMessageService messageService;

        public MessageReceiverFactory(
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

        public MessageReceiver Create()
        {
            var receiver = new MessageReceiver(this.packetService, this.payloadService, this.protocolService, this.messageService);
            return receiver;
        }
    }
}