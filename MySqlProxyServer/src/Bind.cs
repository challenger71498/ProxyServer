using System.Collections.Generic;
using Min.MySqlProxyServer.Protocol;
using Min.MySqlProxyServer.Sockets;
using SimpleInjector;

namespace Min.MySqlProxyServer
{
    public class Bind
    {
        public static Container Create()
        {
            var container = new Container();

            container.RegisterSingleton<IPacketService, PacketService>();
            container.RegisterSingleton<IPayloadService, PayloadService>();
            container.RegisterSingleton<IProtocolService, ProtocolService>();
            container.RegisterSingleton<IMessageService, MessageService>();

            container.RegisterSingleton<MessageSenderFactory>();
            container.RegisterSingleton<MessageReceiverFactory>();

            container.RegisterSingleton<PacketFactory>();

            container.RegisterSingleton<SocketControllerFactory>();

            container.RegisterSingleton<ProxyFactory>();
            container.RegisterSingleton<ServerFactory>();

            // container.Verify();

            return container;
        }
    }
}