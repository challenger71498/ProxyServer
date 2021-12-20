using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class ProtocolReceiverFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;

        public ProtocolReceiverFactory(
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
        }

        public PayloadReceiverService Create()
        {
            var receiver = new PayloadReceiverService(this.packetService, this.payloadService);
            return receiver;
        }
    }
}