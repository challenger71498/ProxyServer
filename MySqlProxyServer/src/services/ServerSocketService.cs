namespace Min.MySqlProxyServer.Sockets
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class ServerSocketService
    {
        private readonly Socket socket;
        private SocketConnection connection;

        public IPEndPoint EndPoint { get; private set; }

        public ServerSocketService(IPEndPoint serverEndPoint)
        {
            this.EndPoint = serverEndPoint;

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task<SocketConnection> GetConnection()
        {
            try
            {
                await this.socket.ConnectAsync(this.EndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when start connecting... {e.GetType()} {e.Message}");
                Console.WriteLine($"STACKTRACE: {e.StackTrace}");
            }

            this.connection = new SocketConnection(this.socket);

            return this.connection;
        }
    }
}