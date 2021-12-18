// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class Server
    {
        private readonly ClientSocketService clientSocketService;
        private readonly ServerSocketService serverSocketService;
        private readonly ProxyFactory proxyFactory;

        public Server(
            ClientSocketService clientSocketService,
            ServerSocketService serverSocketService,
            ProxyFactory proxyFactory)
        {
            this.clientSocketService = clientSocketService;
            this.serverSocketService = serverSocketService;

            this.proxyFactory = proxyFactory;
        }

        public async Task Start()
        {
            var whenClientConnected = this.clientSocketService.StartListening();

            if (whenClientConnected == null)
            {
                throw new Exception("Client socket observable is null.");
            }

            Console.WriteLine($"Server has been succesfully started on port {this.clientSocketService.EndPoint.Port}");

            await whenClientConnected.Do(this.OnClientConnected);

            Console.WriteLine("Cya!");
        }

        private async void OnClientConnected(ISocketConnection client)
        {
            var server = await this.serverSocketService.GetConnection();

            if (server == null)
            {
                // TODO: Exception handling when cannot create server.
                Console.WriteLine("Cannot create server. Creating Proxy failed.");
                return;
            }

            // TODO: Handle proxy lifecycle.
            var proxy = this.proxyFactory.Create(client, server);

            Console.WriteLine("Client socket has been connected");
        }
    }
}
