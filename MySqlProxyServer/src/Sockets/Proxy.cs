// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class Proxy
    {
        private readonly SocketHandler client;
        private readonly SocketHandler server;

        private readonly PacketService clientPacketService;
        private readonly PacketService serverPacketService;

        public Proxy(SocketConnection clientConnection, SocketConnection serverConnection)
        {
            this.clientPacketService = new PacketService();
            this.serverPacketService = new PacketService();

            var clientProtocolFactories = new IProtocolFactory[]
            {
                new QueryCommandFactory(),
                new HandShakeResponseFactory(),
            };

            var clientProtocolProcessors = new IProtocolProcessor[]
            {
                new QueryCommandProcessor(),
                new HandshakeResponseProcessor(),
            };

            this.client = new SocketHandler(clientConnection, this.clientPacketService, new PayloadService(clientProtocolFactories, clientProtocolProcessors));
            this.server = new SocketHandler(serverConnection, this.serverPacketService);

            this.client.PacketsReadyEventHandler += this.OnClientPacketsReady;
            this.server.PacketsReadyEventHandler += this.OnServerPacketsReady;

            this.client.DisconnectedEventHandler += this.OnClientDisconnected;
            this.server.DisconnectedEventHandler += this.OnServerDisconnected;

            // Console.WriteLine("Proxy has been successfully created");
        }

        private async void OnClientPacketsReady(object? sender, byte[][] packets)
        {
            // Console.WriteLine("Trying to send client packets to the server...");

            foreach (var packet in packets)
            {
                await this.server.Send(packet);
            }

            Console.WriteLine("Client packets are sent to the server.");
        }

        private async void OnServerPacketsReady(object? sender, byte[][] packets)
        {
            // Console.WriteLine("Trying to send server packets to the client...");

            foreach (var packet in packets)
            {
                await this.client.Send(packet);
            }

            Console.WriteLine("Server packets are sent to the client.");
        }

        private void OnClientDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Client has been disconnected!");
            this.server.Disconnect();
        }

        private void OnServerDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Server has been disconnected!");
        }
    }
}
