// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class Proxy
    {
        private readonly SocketController client;
        private readonly SocketController server;

        private readonly PacketService clientPacketService;
        private readonly PacketService serverPacketService;

        public Proxy(ISocketConnection clientConnection, ISocketConnection serverConnection)
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

            this.client = new SocketController(clientConnection, new MessageSender(), new MessageReceiver());
            this.server = new SocketController(serverConnection, new MessageSender(), new MessageReceiver());

            this.server.SetMessageReceiveStream(this.client.WhenMessageCreated);
            this.client.SetMessageReceiveStream(this.server.WhenMessageCreated);
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
