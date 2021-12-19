using System;

namespace Min.MySqlProxyServer.Sockets
{
    public class Proxy
    {
        private readonly IConnectionDelegator client;
        private readonly IConnectionDelegator server;

        public Proxy(
            IConnectionDelegator client,
            IConnectionDelegator server)
        {
            this.client = client;
            this.server = server;

            this.server.SetMessageReceiveStream(this.client.WhenMessageCreated);
            this.client.SetMessageReceiveStream(this.server.WhenMessageCreated);
        }

        private void OnClientDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Client has been disconnected!");
            // this.server.Disconnect(); TODO: Handle client disconnection.
        }

        private void OnServerDisconnected(object? sender, EventArgs e)
        {
            Console.WriteLine("Server has been disconnected!");
        }
    }
}
