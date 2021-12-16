// Copyright (c) Min. All rights reserved.

using System;

namespace Min.MySqlProxyServer.Sockets
{
    /// <summary>
    /// <see cref="ISocketConnection"/> is an interface for socket connection.
    /// </summary>
    public interface ISocketConnection
    {
        /// <summary>
        /// Invoked when a socket connection has been disconnected.
        /// </summary>
        event EventHandler<EventArgs>? DisconnectedEventHandler;

        /// <summary>
        /// An observable data stream.
        /// </summary>
        IObservable<byte[]> DataObservable { get; }

        /// <summary>
        /// Gets a value indicating whether the socket is connected.
        /// </summary>
        /// <value>Boolean value of connectivity.</value>
        bool Connected { get; }
    }
}