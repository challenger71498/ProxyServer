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

            container.Collection.Register<IProtocolFactory>(typeof(HandShakeResponseFactory), typeof(AuthSwitchResponseFactory));

            container.RegisterSingleton<IPacketService, PacketService>();
            container.RegisterSingleton<IPayloadService, PayloadService>();
            container.RegisterSingleton<IProtocolService, ProtocolService>();
            container.RegisterSingleton<IMessageService, MessageService>();
            container.RegisterSingleton<AuthService>();

            container.RegisterSingleton<ProtocolSenderFactory>();
            container.RegisterSingleton<ProtocolReceiverFactory>();

            // container.RegisterSingleton<ClientMessageSenderFactory>();
            // container.RegisterSingleton<ClientMessageReceiverFactory>();

            // container.RegisterSingleton<ServerMessageSenderFactory>();
            // container.RegisterSingleton<ServerMessageReceiverFactory>();

            container.RegisterSingleton<PacketFactory>();

            container.RegisterSingleton<IConnectionDelegatorFactory, ConnectionDelegatorFactory>();

            container.RegisterSingleton<ProxyFactory>();
            container.RegisterSingleton<ServerFactory>();

            // container.Verify();

            return container;
        }
    }
}