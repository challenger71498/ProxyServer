using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public interface IMessageSenderFactory
    {
        PayloadSenderService Create();
    }

    // NOTE: Need?
    public class PayloadSenderFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;

        public PayloadSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
        }

        public PayloadSenderService Create()
        {
            var sender = new PayloadSenderService(this.packetService, this.payloadService);
            return sender;
        }
    }

    // public class ClientMessageSenderFactory : ProtocolSenderFactory
    // {
    //     public ClientMessageSenderFactory(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     /// <inheritdoc/>
    //     public override ProtocolSender Create()
    //     {
    //         var sender = new ClientMessageSender(this.packetService, this.payloadService, this.protocolService);
    //         return sender;
    //     }
    // }

    // public class ServerMessageSenderFactory : ProtocolSenderFactory
    // {
    //     public ServerMessageSenderFactory(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     /// <inheritdoc/>
    //     public override ProtocolSender Create()
    //     {
    //         var sender = new ServerMessageSender(this.packetService, this.payloadService, this.protocolService);
    //         return sender;
    //     }
    // }

    // public interface IMessageReceiverFactory
    // {
    //     ProtocolReceiver Create();
    // }

    // public class ClientMessageReceiverFactory : BaseMessageReceiverFactory
    // {
    //     private readonly AuthService authService;

    //     public ClientMessageReceiverFactory(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService,
    //         AuthService authService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //         this.authService = authService;
    //     }

    //     /// <inheritdoc/>
    //     public override ProtocolReceiver Create()
    //     {
    //         var receiver = new ClientMessageReceiver(this.packetService, this.payloadService, this.protocolService, this.authService);
    //         return receiver;
    //     }
    // }

    // public class ServerMessageReceiverFactory : BaseMessageReceiverFactory
    // {
    //     public ServerMessageReceiverFactory(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     /// <inheritdoc/>
    //     public override ProtocolReceiver Create()
    //     {
    //         var receiver = new ServerMessageReceiver(this.packetService, this.payloadService, this.protocolService);
    //         return receiver;
    //     }
    // }

    // NOTE: Need?
    public class ProtocolReceiverFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;

        public ProtocolReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
        }

        public PayloadReceiverService Create()
        {
            var receiver = new PayloadReceiverService(this.packetService, this.payloadService);
            return receiver;
        }
    }
}