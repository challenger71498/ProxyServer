using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public interface IMessageSenderFactory
    {
        MessageSender Create();
    }

    // NOTE: Need?
    public abstract class BaseMessageSenderFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;
        protected readonly IProtocolService protocolService;

        public BaseMessageSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public abstract MessageSender Create();
    }

    public class ClientMessageSenderFactory : BaseMessageSenderFactory
    {
        public ClientMessageSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
            : base(packetService, payloadService, protocolService)
        {
        }

        /// <inheritdoc/>
        public override MessageSender Create()
        {
            var sender = new ClientMessageSender(this.packetService, this.payloadService, this.protocolService);
            return sender;
        }
    }

    public class ServerMessageSenderFactory : BaseMessageSenderFactory
    {
        public ServerMessageSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
            : base(packetService, payloadService, protocolService)
        {
        }

        /// <inheritdoc/>
        public override MessageSender Create()
        {
            var sender = new ServerMessageSender(this.packetService, this.payloadService, this.protocolService);
            return sender;
        }
    }

    public interface IMessageReceiverFactory
    {
        MessageReceiver Create();
    }

    public class ClientMessageReceiverFactory : BaseMessageReceiverFactory
    {
        private readonly AuthService authService;

        public ClientMessageReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService,
            AuthService authService)
            : base(packetService, payloadService, protocolService)
        {
            this.authService = authService;
        }

        /// <inheritdoc/>
        public override MessageReceiver Create()
        {
            var receiver = new ClientMessageReceiver(this.packetService, this.payloadService, this.protocolService, this.authService);
            return receiver;
        }
    }

    public class ServerMessageReceiverFactory : BaseMessageReceiverFactory
    {
        public ServerMessageReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
            : base(packetService, payloadService, protocolService)
        {
        }

        /// <inheritdoc/>
        public override MessageReceiver Create()
        {
            var receiver = new ServerMessageReceiver(this.packetService, this.payloadService, this.protocolService);
            return receiver;
        }
    }

    // NOTE: Need?
    public abstract class BaseMessageReceiverFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;
        protected readonly IProtocolService protocolService;

        public BaseMessageReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public abstract MessageReceiver Create();
    }
}