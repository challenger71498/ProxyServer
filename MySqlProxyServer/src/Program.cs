using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class Program
    {
        /// <summary>
        /// Main entrypoint of the program.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Main()
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Loopback, 8080);
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, 3306);

            var clientSocketService = new ClientSocketService(clientEndPoint);

            var packetService = new PacketService();
            var payloadService = new PayloadService();

            packetService.OnPayload += (object sender, byte[] payload) =>
            {
                var protocol = payloadService.GetProtocol(payload);

                if (protocol != null && protocol.GetType() == typeof(Handshake))
                {
                }
            };

            clientSocketService.AcceptHandler += async (object sender, Socket socket) =>
            {
                Console.WriteLine("Socket client has been connected!");
                var client = new SocketConnection(socket);

                var serverSocketService = new ServerSocketService(serverEndPoint);
                var server = await serverSocketService.GetConnection();

                server.OnReceiveData += async (object sender, byte[] data) =>
                {
                    // Console.WriteLine($"From server to client: {data.Length}");

                    // Analyze data.
                    packetService.PushPacket(data);

                    await client.Send(data);
                };

                client.OnReceiveData += async (object sender, byte[] data) =>
                {
                    // Console.WriteLine($"From client to server: {data.Length}");
                    await server.Send(data);
                };
            };

            await clientSocketService.StartListening();
        }
    }
}
