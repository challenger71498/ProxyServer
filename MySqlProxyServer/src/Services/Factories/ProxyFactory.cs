namespace Min.MySqlProxyServer.Sockets
{
    public class ProxyFactory
    {
        private readonly IConnectionDelegatorFactory connectionDelegatorFactory;
        private readonly PayloadSenderService protocolSenderService;
        private readonly PayloadReceiverService protocolReceiverService;
        private readonly AuthService authService;
        private readonly ProtocolService protocolService;
        private readonly LoggerService loggerService;

        public ProxyFactory(
            IConnectionDelegatorFactory socketControllerFactory,
            PayloadSenderService protocolSenderService,
            PayloadReceiverService protocolReceiverService,
            AuthService authService,
            ProtocolService protocolService,
            LoggerService loggerService)
        {
            this.connectionDelegatorFactory = socketControllerFactory;
            this.protocolSenderService = protocolSenderService;
            this.protocolReceiverService = protocolReceiverService;

            this.authService = authService;
            this.protocolService = protocolService;
            this.loggerService = loggerService;
        }

        public Proxy Create(ISocketConnection clientConnection, ISocketConnection serverConnection)
        {
            // Note that two connections are reversed.
            // This is because delegator delegates the counterpart of its connection; not the connection itself.
            var serverDelegator = this.connectionDelegatorFactory.Create(clientConnection, this.protocolSenderService, this.protocolReceiverService);
            var clientDelegator = this.connectionDelegatorFactory.Create(serverConnection, this.protocolSenderService, this.protocolReceiverService);

            var proxy = new Proxy(clientDelegator, serverDelegator, this.protocolService, this.authService, this.loggerService);
            return proxy;
        }
    }
}