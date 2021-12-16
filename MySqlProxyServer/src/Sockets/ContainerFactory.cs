using Unity;

namespace Min.MySqlProxyServer.Sockets
{
    public static class ContainerFactory
    {
        public static IUnityContainer CreateFakeClientContainer()
        {
            var container = new UnityContainer();

            container.RegisterFactory<AuthModule>((container) =>
            {
                var authModule = new AuthModule(Encryption.HashAlgorithmType.SHA1);
                return authModule;
            });

            container.RegisterSingleton<ISSLModule>();

            return container;
        }
    }

    public interface ISSLModule { }

    public class SSLModule
    {

    }
}