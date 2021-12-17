using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
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

            var server = new Server(clientEndPoint, serverEndPoint);

            await server.Start();
        }
    }
}
