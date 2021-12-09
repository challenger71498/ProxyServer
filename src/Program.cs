using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProxyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer = Convert.FromHexString("00112233");

            var stream = new MemoryStream(buffer);
            var bytes = new byte[1];
            var span = new Span<byte>(bytes);

            while (stream.Read(span) != 0)
            {
                Console.WriteLine(Convert.ToHexString(bytes));
            }



            // var ipEndPoint = new IPEndPoint(IPAddress.Parse("172.17.0.2"), 3306);

            // using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            // {
            //     Console.WriteLine("Starting...");

            //     try
            //     {
            //         client.Connect(ipEndPoint);
            //     }
            //     catch (Exception e)
            //     {
            //         Console.WriteLine("ERROR! ", e.Message);
            //     }

            //     Console.WriteLine("START");
            //     ;
            //     new Task(() =>
            //     {
            //         Console.WriteLine("Starting tasks...");

            //         try
            //         {
            //             while (true)
            //             {
            //                 var binary = new Byte[1024];

            //                 client.Receive(binary);

            //                 var data = Convert.ToHexString(binary);

            //                 if (data.Substring(0, 3) == "000")
            //                 {
            //                     continue;
            //                 }

            //                 Console.WriteLine($"DATA RECEIVED: {data}");
            //             }

            //         }
            //         catch (Exception e)
            //         {
            //             Console.WriteLine($"ERROR! {e.Message}");
            //         }
            //     }).Start();

            //     while (true)
            //     {
            //         // 콘솔 입력 받는다.
            //         var msg = Console.ReadLine();
            //         // 클라이언트로 받은 메시지를 String으로 변환
            //         var output = Convert.FromHexString("0100000001");
            //         client.Send(output);
            //         // 메시지 내용이 exit라면 무한 루프 종료(즉, 클라이언트 종료)
            //         if ("EXIT".Equals(msg, StringComparison.OrdinalIgnoreCase))
            //         {
            //             break;
            //         }
            //     }
            //     // 콘솔 출력 - 접속 종료 메시지
            //     Console.WriteLine($"Disconnected");
            // }
        }
    }
}
