// Copyright (c) Min. All rights reserved.

using System;
using System.Net;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Sockets;
using Unity;

namespace Min.MySqlProxyServer
{
    public class Server
    {
        private readonly ClientSocketService clientSocketService;

        private readonly ServerSocketService serverSocketService;

        public async Task Start()
        {
            var whenClientConnected = this.clientSocketService.StartListening();

            if (whenClientConnected == null)
            {
                throw new Exception("Client socket observable is null.");
            }

            whenClientConnected.Subscribe(this.OnClientConnected);

            Console.WriteLine($"Server has been succesfully started on port {this.clientSocketService.EndPoint.Port}");
        }

        private async void OnClientConnected(ISocketConnection client)
        {
            var server = await this.serverSocketService.GetConnection();

            var proxy = new Proxy(client, server);

            Console.WriteLine("Socket has been connected");
        }
    }
}
