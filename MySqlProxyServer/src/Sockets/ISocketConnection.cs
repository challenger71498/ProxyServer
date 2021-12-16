// Copyright (c) Min. All rights reserved.

using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Min.MySqlProxyServer.Sockets
{
    /// <summary>
    /// <see cref="ISocketConnection"/> is an interface for socket connection.
    /// </summary>
    public interface ISocketConnection
    {
        /// <summary>
        /// Gets an observable that issues when the socket has been disconnected.
        /// </summary>
        /// <remarks>
        /// Returns a boolean value (true), but it describes nothing.
        /// </remarks>
        IObservable<bool> WhenDisconnected { get; }

        /// <summary>
        /// Gets an observable data stream.
        /// </summary>
        IObservable<byte[]> WhenDataReceived { get; }

        /// <summary>
        /// Gets a value indicating whether the socket is connected.
        /// </summary>
        /// <value>Boolean value of connectivity.</value>
        bool Connected { get; }

        /// <summary>
        /// Sends a binary data to the socket.
        /// </summary>
        /// <param name="data">Byte array format of data.</param>
        /// <returns>A task which ends when the sending has finished.</returns>
        Task Send(byte[] data);
    }
}
