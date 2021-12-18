using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;

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

            var container = Bind.Create();

            var server = container.GetInstance<ServerFactory>().Create(clientEndPoint, serverEndPoint);

            await server.Start();
        }
    }
}
