using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class PacketMessage : ISocketControllerMessage
    {
        public PacketMessage(IPacket packet)
        {
            this.Packet = packet;
        }

        public IPacket Packet { get; }
    }
}