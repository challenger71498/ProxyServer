// Copyright (c) Min. All rights reserved.

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class ServerSocketService
    {
        public IPEndPoint EndPoint { get; private set; }

        public ServerSocketService(IPEndPoint serverEndPoint)
        {
            this.EndPoint = serverEndPoint;
        }

        public async Task<SocketConnection?> GetConnection()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                await socket.ConnectAsync(this.EndPoint);
                var connection = new SocketConnection(socket);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when start connecting... {e.GetType()} {e.Message}");
                Console.WriteLine($"STACKTRACE: {e.StackTrace}");
                return null;
            }
        }
    }
}
