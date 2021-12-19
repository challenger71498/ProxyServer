namespace Min.MySqlProxyServer.Sockets
{
    public class ProxyFactory
    {
        private readonly IConnectionDelegatorFactory connectionDelegatorFactory;
        private readonly ClientMessageSenderFactory clientMessageSenderFactory;
        private readonly ClientMessageReceiverFactory clientMessageReceiverFactory;
        private readonly ServerMessageSenderFactory serverMessageSenderFactory;
        private readonly ServerMessageReceiverFactory serverMessageReceiverFactory;

        public ProxyFactory(
            IConnectionDelegatorFactory socketControllerFactory,
            ClientMessageSenderFactory clientMessageSenderFactory,
            ClientMessageReceiverFactory clientMessageReceiverFactory,
            ServerMessageSenderFactory serverMessageSenderFactory,
            ServerMessageReceiverFactory serverMessageReceiverFactory)
        {
            this.connectionDelegatorFactory = socketControllerFactory;

            this.clientMessageSenderFactory = clientMessageSenderFactory;
            this.clientMessageReceiverFactory = clientMessageReceiverFactory;

            this.serverMessageSenderFactory = serverMessageSenderFactory;
            this.serverMessageReceiverFactory = serverMessageReceiverFactory;
        }

        public Proxy Create(ISocketConnection clientConnection, ISocketConnection serverConnection)
        {
            var clientSender = this.clientMessageSenderFactory.Create();
            var clientReceiver = this.clientMessageReceiverFactory.Create();

            var serverSender = this.serverMessageSenderFactory.Create();
            var serverReceiver = this.serverMessageReceiverFactory.Create();

            // Note that two connections are reversed.
            // This is because delegator delegates the counterpart of its connection; not the connection itself.
            var serverDelegator = this.connectionDelegatorFactory.Create(clientConnection, serverSender, serverReceiver);
            var clientDelegator = this.connectionDelegatorFactory.Create(serverConnection, clientSender, clientReceiver);

            var proxy = new Proxy(serverDelegator, clientDelegator);
            return proxy;
        }
    }
}