using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class PayloadSenderFactory
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;

        public PayloadSenderFactory(
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
        }

        public PayloadSenderService Create()
        {
            var sender = new PayloadSenderService(this.packetService, this.payloadService);
            return sender;
        }
    }
}