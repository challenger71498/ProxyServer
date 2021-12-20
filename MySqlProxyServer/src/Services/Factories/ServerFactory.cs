using System.Net;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class ServerFactory
    {
        private readonly ProxyFactory proxyFactory;

        public ServerFactory(ProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
        }

        public Server Create(IPEndPoint clientEndPoint, IPEndPoint serverEndPoint)
        {
            var clientSocketService = new ClientSocketService(clientEndPoint);
            var serverSocketService = new ServerSocketService(serverEndPoint);

            var server = new Server(clientSocketService, serverSocketService, this.proxyFactory);
            return server;
        }
    }
}