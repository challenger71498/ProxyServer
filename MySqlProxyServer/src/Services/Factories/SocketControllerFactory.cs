namespace Min.MySqlProxyServer.Sockets
{
    public interface ISocketControllerFactory
    {
        SocketController Create(ISocketConnection connection);
    }

    public class SocketControllerFactory : ISocketControllerFactory
    {
        private readonly MessageSenderFactory senderFactory;
        private readonly MessageReceiverFactory receiverFactory;

        public SocketControllerFactory(
            MessageSenderFactory senderFactory,
            MessageReceiverFactory receiverFactory)
        {
            this.senderFactory = senderFactory;
            this.receiverFactory = receiverFactory;
        }

        /// <inheritdoc/>
        public SocketController Create(ISocketConnection connection)
        {
            var sender = this.senderFactory.Create();
            var receiver = this.receiverFactory.Create();

            var controller = new SocketController(connection, sender, receiver);
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