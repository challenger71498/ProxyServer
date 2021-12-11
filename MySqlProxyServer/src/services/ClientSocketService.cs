// Copyright (c) Min. All rights reserved.

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Min.MySqlProxyServer.Sockets
{
    /// <summary>
    /// ClientSocketService opens socket connection on the end point.
    /// </summary>
    public class ClientSocketService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSocketService"/> class.
        /// </summary>
        /// <param name="endPoint">End point.</param>
        public ClientSocketService(IPEndPoint endPoint)
        {
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Event handler for socket accept.
        /// </summary>
        public event EventHandler<Socket> AcceptHandler;

        /// <summary>
        /// Gets ip end point.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// Start listening socket connection on the end point.
        /// </summary>
        /// <returns>A Task which ends on error.</returns>
        public async Task StartListening()
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(this.EndPoint);
                listener.Listen();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    var accepted = await listener.AcceptAsync();
                    this.AcceptHandler?.Invoke(this, accepted);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR! {e.Message}");
            }
        }
    }
}
