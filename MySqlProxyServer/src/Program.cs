namespace MySql.ProxyServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using MySql.ProxyServer.Protocol;

    class Program
    {
        public static void Main(string[] args)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("172.17.0.2"), 3306);

            using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Starting...");

            try
            {
                client.Connect(ipEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR! {e.Message}");
            }

            Console.WriteLine("START");
            ;
            new Task(async () =>
            {
                Console.WriteLine("Starting tasks...");

                try
                {
                    while (true)
                    {
                        var binary = new byte[1024];

                        await client.ReceiveAsync(binary, SocketFlags.None);

                        var adapter = new PacketAdapter(binary);

                        if (adapter.PayloadLength == 0)
                        {
                            continue;
                        }

                        var payload = Convert.ToHexString(adapter.Payload);

                        Console.WriteLine(adapter.PayloadLength);
                        Console.WriteLine($"DATA RECEIVED: {payload}");

                        var handshake = new HandshakeProtocol(adapter.Payload);

                        Console.WriteLine($"AuthPluginData: {handshake.AuthPluginData}");
                        Console.WriteLine($"AuthPluginName: {handshake.AuthPluginName}");
                        Console.WriteLine($"Capability: {handshake.Capability}");
                        Console.WriteLine($"CharacterSet: {handshake.CharacterSet}");
                        Console.WriteLine($"ConnectionId: {handshake.ConnectionId}");
                        Console.WriteLine($"ProtocolVersion: {handshake.ProtocolVersion}");
                        Console.WriteLine($"ServerVersion: {handshake.ServerVersion}");
                        Console.WriteLine($"StatusFlag: {handshake.StatusFlag}");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR! {e.Message}");
                }
            }).Start();

            while (true)
            {
                // 콘솔 입력 받는다.
                var msg = Console.ReadLine();
                // 클라이언트로 받은 메시지를 String으로 변환
                var output = Convert.FromHexString("0100000001");
                client.Send(output);
                // 메시지 내용이 exit라면 무한 루프 종료(즉, 클라이언트 종료)
                if ("EXIT".Equals(msg, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
            // 콘솔 출력 - 접속 종료 메시지
            Console.WriteLine($"Disconnected");
        }
    }
}
