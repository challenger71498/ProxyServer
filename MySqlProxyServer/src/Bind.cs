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

            // default factories
            container.Collection.Register<IProtocolFactory>(typeof(HandShakeResponseFactory), typeof(QueryCommandFactory));

            container.RegisterSingleton<IPacketService, PacketService>();
            container.RegisterSingleton<IPayloadService, PayloadService>();
            container.RegisterSingleton<ProtocolService>();
            container.RegisterSingleton<AuthService>();
            container.RegisterSingleton<LoggerService>();

            container.RegisterSingleton<PayloadSenderService>();
            container.RegisterSingleton<PayloadReceiverService>();

            container.RegisterSingleton<PacketFactory>();

            container.RegisterSingleton<IConnectionDelegatorFactory, ConnectionDelegatorFactory>();

            container.RegisterSingleton<ProxyFactory>();
            container.RegisterSingleton<ServerFactory>();

            // container.Verify();

            return container;
        }
    }
}