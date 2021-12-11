namespace Min.MySqlProxyServer.Sockets
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class SocketConnection
    {
        public Socket socket;

        public SocketConnection(Socket socket)
        {
            this.socket = socket;

            this.StartListening();
        }

        public event EventHandler<byte[]> OnReceiveData;

        public async Task Send(byte[] binary)
        {
            await this.socket.SendAsync(binary, SocketFlags.None);
        }

        private async Task StartListening()
        {
            var listenTask = new Task(async () =>
            {
                try
                {
                    while (true)
                    {
                        var buffer = new byte[1024 * 1024 * 16];
                        var length = await this.socket.ReceiveAsync(buffer, SocketFlags.None);

                        if (length == 0)
                        {
                            continue;
                        }

                        this.OnReceiveData?.Invoke(this, buffer[..length]);
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