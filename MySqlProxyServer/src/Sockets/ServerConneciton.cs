using System.IO;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class ServerConnection : MySqlConnection
    {
        public ServerConnection(SocketConnection connection, CapabilityFlag capability)
            : base(connection, capability)
        {
        }

        protected override IProtocol? GetProtocol(byte[] data)
        {
            var handShakeFactory = new HandShakeFactory();

            if (handShakeFactory.TryCreate(data, out var handshake))
            {
                System.Console.WriteLine($"handshake.AuthPluginData: {handshake.AuthPluginData}");
                System.Console.WriteLine($"handshake.AuthPluginName: {handshake.AuthPluginName}");
                System.Console.WriteLine($"handshake.Capability: {handshake.Capability}");
                System.Console.WriteLine($"handshake.CharacterSet: {handshake.CharacterSet}");
                System.Console.WriteLine($"handshake.ConnectionId: {handshake.ConnectionId}");
                System.Console.WriteLine($"handshake.ProtocolVersion: {handshake.ProtocolVersion}");
                System.Console.WriteLine($"handshake.ServerVersion: {handshake.ServerVersion}");
                System.Console.WriteLine($"handshake.StatusFlag: {handshake.StatusFlag}");

                return handshake;
            }

            System.Console.WriteLine("Failed to convert to handshake!");
            return null;
        }
    }
}
