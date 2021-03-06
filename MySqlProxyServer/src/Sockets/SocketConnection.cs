using System;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
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
        private readonly CancellationTokenSource socketCancellationTokenSource = new();

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

            // Console.WriteLine($"Sending: {System.Text.Encoding.ASCII.GetString(binary[..Math.Min(200, binary.Length)])}");
            // Console.WriteLine($"Sending: {Convert.ToHexString(binary[..Math.Min(200, binary.Length)])}");
            // Console.WriteLine($"LEN: {binary.Length}");

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

            // TODO: Close socket gracefully.
            this.socketCancellationTokenSource.Cancel();
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
            // TODO: Emit stream only once.
            var subject = new Subject<bool>();

            var disconnectedStream = Observable.Interval(TimeSpan.FromMilliseconds(1000))
                .TakeWhile(_ => this.Connected)
                .Select((_) =>
                {
                    var polled = this.socket.Poll(1000, SelectMode.SelectRead);
                    var zero = this.socket.Available == 0;
                    var connected = this.socket.Connected;

                    return (polled && zero) || !connected;
                })
                .Where(disconnected => disconnected)
                .Multicast(subject);

            subject.Subscribe(_ => this.Disconnect());

            disconnectedStream.Connect();

            return subject;
        }

        private IObservable<byte[]> GetDataStream()
        {
            var buffer = new byte[(1024 * 1024 * 16) + 4];

            // TODO: Handle stream on disconnected.
            var dataStream = Observable
                .FromAsync(async () =>
                {
                    if (this.socketCancellationTokenSource.IsCancellationRequested)
                    {
                        return await new Task<int>(() => -1);
                    }

                    return await this.socket.ReceiveAsync(buffer, SocketFlags.None, this.socketCancellationTokenSource.Token);
                })
                .Where(received => received != 0)
                .Select(received => buffer[..received])
                .Catch<byte[], Exception>(this.OnDataStreamError);

            return dataStream;
        }

        private IObservable<byte[]> OnDataStreamError(Exception e)
        {
            // TODO: Exception handling
            Console.WriteLine(e.Message);

            return Observable.Empty<byte[]>();
        }
    }
}
