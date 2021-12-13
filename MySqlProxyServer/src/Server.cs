// Copyright (c) Min. All rights reserved.

using System;
using System.Net;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class Server
    {
        private readonly ClientSocketService clientSocketService;
        private readonly ServerSocketService serverSocketService;

        public Server(IPEndPoint clientEndPoint, IPEndPoint serverEndPoint)
        {
            this.clientSocketService = new ClientSocketService(clientEndPoint);
            this.serverSocketService = new ServerSocketService(serverEndPoint);
        }

        public async Task Start()
        {
            this.clientSocketService.ConnectedEventHandler += this.OnClientSocketConnected;

            Console.WriteLine($"Server has been succesfully started on port {this.clientSocketService.EndPoint.Port}");

            await this.clientSocketService.StartListening();
        }

        private async void OnClientSocketConnected(object? sender, SocketConnection client)
        {
            var server = await this.serverSocketService.GetConnection();

            var proxy = new Proxy(client, server);

            Console.WriteLine("Socket has been connected");
        }
    }
}
