namespace Min.MySqlProxyServer.Sockets
{
    public class ProxyFactory
    {
        private readonly SocketControllerFactory factory;

        public ProxyFactory(
            SocketControllerFactory factory)
        {
            this.factory = factory;
        }

        public Proxy Create(ISocketConnection clientConnection, ISocketConnection serverConnection)
        {
            var client = this.factory.Create(clientConnection);
            var server = this.factory.Create(serverConnection);

            var proxy = new Proxy(client, server);
            return proxy;
        }
    }
}