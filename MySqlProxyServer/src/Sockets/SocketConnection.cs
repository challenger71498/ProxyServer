// Copyright (c) Min. All rights reserved.

using System;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Min.MySqlProxyServer.Sockets
{
    /// <summary>
    /// A <see cref="SocketConnection" /> class handles socket connection, providing interfaces to interface with.
    /// </summary>
    public class SocketConnection : ISocketConnection
    {
        private const int SocketKeepAliveTime = 2;
        private const int SocketKeepAliveInterval = 1;
        private const int SocketKeepAliveRetryCount = 1;

        private readonly Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketConnection"/> class.
        /// </summary>
        /// <param name="socket">A socket.</param>
        public SocketConnection(Socket socket)
        {
            this.socket = socket;

            this.Connected = true;

            this.SetKeepAliveOptions(SocketKeepAliveTime, SocketKeepAliveInterval, SocketKeepAliveRetryCount);

            this.WhenDisconnected = this.GetDisconnectedStream();
            this.WhenDataReceived = this.GetDataStream();
        }

        /// <inheritdoc [cref="ISocketConnection"] />
        public bool Connected { get; private set; }

        /// <inheritdoc [cref="ISocketConnection"] />
        public IObservable<bool> WhenDisconnected { get; private set; }

        /// <inheritdoc [cref="ISocketConnection"] />
        public IObservable<byte[]> WhenDataReceived { get; private set; }

        /// <inheritdoc [cref="ISocketConnection"] />
        public async Task Send(byte[] binary)
        {
            // Console.WriteLine($"Sending: {System.Text.Encoding.ASCII.GetString(binary)}");
            // Console.WriteLine($"Sending: {Convert.ToHexString(binary)}");

            if (!this.Connected)
            {
                Console.WriteLine("Failed to send!");
                return;
            }

            await this.socket.SendAsync(binary, SocketFlags.None);
        }

        /// <summary>
        /// Disconnect the socket.
        /// </summary>
        public void Disconnect()
        {
            if (!this.Connected)
            {
                return;
            }

            this.Connected = false;

            this.socket.Shutdown(SocketShutdown.Receive);
            this.socket.Close();
        }

        private void SetKeepAliveOptions(int time, int interval, int retryCount)
        {
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, time);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, interval);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, retryCount);
        }

        private IObservable<bool> GetDisconnectedStream()
        {
            var disconnectedStream = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .Select((_) =>
                {
                    var polled = this.socket.Poll(1000, SelectMode.SelectRead);
                    var available = this.socket.Available == 0;
                    var connected = this.socket.Connected;

                    return (polled && available) || !connected;
                })
                .Where(disconnected => disconnected == true);

            return disconnectedStream;
        }

        private IObservable<byte[]> GetDataStream()
        {
            var buffer = new byte[(1024 * 1024 * 16) + 4];

            var dataStream = Observable
                .FromAsync(() => this.socket.ReceiveAsync(buffer, SocketFlags.None))
                .Where((received) => received != 0)
                .Select((_) => buffer);

            dataStream.Catch<byte[], Exception>(this.OnDataStreamError);

            return dataStream;
        }

        private IObservable<byte[]> OnDataStreamError(Exception e)
        {
            // TODO: Error handling

            return Observable.Empty<byte[]>();
        }
    }
}
