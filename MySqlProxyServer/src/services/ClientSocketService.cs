// Copyright (c) Min. All rights reserved.

using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;

namespace Min.MySqlProxyServer.Sockets
{
    /// <summary>
    /// ClientSocketService opens socket connection on the end point.
    /// </summary>
    public class ClientSocketService
    {
        private Socket? listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSocketService"/> class.
        /// </summary>
        /// <param name="endPoint">End point.</param>
        public ClientSocketService(IPEndPoint endPoint)
        {
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ClientSocketService"/> class.
        /// Also disposes listener.
        /// </summary>
        ~ClientSocketService()
        {
            this.listener?.Dispose();
        }

        /// <summary>
        /// Gets the IP end point.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// Start listening socket connection on the end point.
        /// </summary>
        /// <returns>IObservable that issues an event when a new socket has been connected.</returns>
        public IObservable<SocketConnection>? StartListening()
        {
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.listener.Bind(this.EndPoint);
            this.listener.Listen();

            var observable = Observable
                .FromAsync(this.listener.AcceptAsync)
                .Select(socket => new SocketConnection(socket));

            observable.Subscribe(
                (_) =>
                {
                    Console.WriteLine("A new client has been connected.");
                },
                (_) =>
                {
                    Console.SetError("asdf");
                    this.listener.Dispose();
                });

            return observable;
        }
    }
}
