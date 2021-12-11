namespace Min.MySqlProxyServer.Tests
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Min.MySqlProxyServer.Sockets;
    using NUnit.Framework;

    [TestFixture]
    public class SocketListenerTest
    {
        private ClientSocketService listener;
        private Socket client;
        private IPEndPoint endPoint;

        /// <summary>
        /// Setup for SocketListener test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.endPoint = new IPEndPoint(IPAddress.Loopback, 8080);
            this.listener = new ClientSocketService(this.endPoint);
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Test for StartListening.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [Test]
        public async Task StartListeningTest()
        {
            var acceptTask = new Task(() =>
            {
                Console.WriteLine("Recived socket.");
            });

            this.listener.StartListening();
            this.listener.AcceptHandler += (object? sender, Socket socket) => acceptTask.Start();

            this.client.Connect(this.endPoint);

            var success = await Task.WhenAny(acceptTask, Task.Delay(1000)) == acceptTask;

            Assert.IsTrue(success, $"Failed to receive socket.");
        }

        [Test]
        public async Task StartMultiClientTest()
        {
            this.listener.StartListening();
            this.listener.AcceptHandler += (object? sender, Socket socket) =>
            {

            };

            using var client1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            using var client2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client1.Connect(this.endPoint);
            client2.Connect(this.endPoint);
        }
    }
}