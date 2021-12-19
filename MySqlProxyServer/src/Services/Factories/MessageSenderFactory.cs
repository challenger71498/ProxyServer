using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public interface IMessageSenderFactory
    {
        ProtocolSender Create();
    }

    // NOTE: Need?
    public class ProtocolSenderFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;
        protected readonly IProtocolService protocolService;

        public ProtocolSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public ProtocolSender Create()
        {
            var sender = new ProtocolSender(this.packetService, this.payloadService, this.protocolService);
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
        protected readonly IProtocolService protocolService;

        public ProtocolReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public ProtocolReceiver Create()
        {
            var receiver = new ProtocolReceiver(this.packetService, this.payloadService, this.protocolService);
            return receiver;
        }
    }
}