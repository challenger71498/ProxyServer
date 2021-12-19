namespace Min.MySqlProxyServer.Sockets
{
    public interface IConnectionDelegatorFactory
    {
        // SocketController Create(ISocketConnection connection);
        ConnectionDelegator Create(ISocketConnection counterConnection, ProtocolSender sender, ProtocolReceiver receiver);
    }

    public class ConnectionDelegatorFactory : IConnectionDelegatorFactory
    {
        // private readonly IMessageSenderFactory senderFactory;
        // private readonly IMessageReceiverFactory receiverFactory;

        // public SocketControllerFactory(
        //     IMessageSenderFactory senderFactory,
        //     IMessageReceiverFactory receiverFactory)
        // {
        //     this.senderFactory = senderFactory;
        //     this.receiverFactory = receiverFactory;
        // }

        /// <inheritdoc/>
        public ConnectionDelegator Create(ISocketConnection counterConnection, ProtocolSender sender, ProtocolReceiver receiver)
        {
            // var sender = this.senderFactory.Create();
            // var receiver = this.receiverFactory.Create();

            var controller = new ConnectionDelegator(counterConnection, sender, receiver);
            return controller;
        }
    }

    // public class ClientSocketControllerFactory : BaseSocketControllerFactory
    // {
    //     public ClientSocketControllerFactory(
    //         [Named(Container.Client)] MessageSender sender,
    //         [Named(Container.Client)] MessageReceiver receiver)
    //         : base(sender, receiver)
    //     {
    //     }
    // }

    // public class ServerSocketControllerFactory : BaseSocketControllerFactory
    // {
    //     public ServerSocketControllerFactory(
    //         [Named(Container.Server)] MessageSender sender,
    //         [Named(Container.Server)] MessageReceiver receiver)
    //         : base(sender, receiver)
    //     {
    //     }
    // }
}