// Copyright (c) Min. All rights reserved.

using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Min.MySqlProxyServer.Sockets
{
    public class SocketConnection
    {
        private const int SocketKeepAliveTime = 2;
        private const int SocketKeepAliveInterval = 1;
        private const int SocketKeepAliveRetryCount = 1;

        private Socket socket; // TODO: Change to private

        public SocketConnection(Socket socket)
        {
            this.socket = socket;

            this.Connected = true;

            this.SetKeepAliveOptions(SocketKeepAliveTime, SocketKeepAliveInterval, SocketKeepAliveRetryCount);
            this.SetDisconnectedEvent();

            this.StartListening();
        }

        public IObservable<byte[]> DataStream { get; }

        public event EventHandler<EventArgs>? DisconnectedEventHandler;

        public event EventHandler<byte[]>? DataReceivedEventHandler;

        public bool Connected { get; private set; }

        public async Task Send(byte[] binary)
        {
            Console.WriteLine($"Sending: {System.Text.Encoding.ASCII.GetString(binary)}");
            Console.WriteLine($"Sending: {Convert.ToHexString(binary)}");

            if (!this.Connected)
            {
                Console.WriteLine("Failed to send!");
                return;
            }

            await this.socket.SendAsync(binary, SocketFlags.None);
        }

        public void Disconnect()
        {
            if (!this.Connected)
            {
                return;
            }

            this.Connected = false;

            this.socket.Shutdown(SocketShutdown.Receive);
            this.socket.Close();

            this.DisconnectedEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void SetKeepAliveOptions(int time, int interval, int retryCount)
        {
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, time);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, interval);
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, retryCount);
        }

        private async void SetDisconnectedEvent()
        {
            try
            {
                while (true)
                {
                    if ((this.socket.Poll(1000, SelectMode.SelectRead) && this.socket.Available == 0) || !this.socket.Connected)
                    {
                        throw new Exception("Socket is not connected.");
                    }

                    await Task.Delay(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Disconnected! {e.GetType()} {e.Message}");

                if (this.Connected)
                {
                    this.Disconnect();
                }
            }
        }

        private void StartListening()
        {
            var listenTask = new Task(async () =>
            {
                try
                {
                    var buffer = new byte[(1024 * 1024 * 16) + 4];

                    while (this.Connected)
                    {
                        var length = await this.socket.ReceiveAsync(buffer, SocketFlags.None);

                        if (length == 0)
                        {
                            continue;
                        }

                        this.DataReceivedEventHandler?.Invoke(this, buffer[..length]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error on receiving... {e.GetType()} {e.Message}");
                    Console.WriteLine($"STACKTRACE: {e.StackTrace}");
                }
            });

            listenTask.Start();
        }
    }
}
