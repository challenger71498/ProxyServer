using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class PayloadSenderService
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;

        public PayloadSenderService(
            IPacketService packetService,
            IPayloadService payloadService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
        }

        public IObservable<IData> GetPayloadStream(IObservable<byte[]> dataStream)
        {
            var payloadStream = dataStream
                .Select(binary => new BinaryData(binary))
                .Let(this.packetService.FromBinaryData)
                .Let(this.payloadService.FromPacket);

            return payloadStream;
        }
    }

    // public class ClientMessageSender : ProtocolSender
    // {
    //     public ClientMessageSender(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     protected override ISocketControllerMessage OnDataReceived(IData data)
    //     {
    //         if (data is IBinaryData binaryData)
    //         {
    //             var message = new RawDataMessage(binaryData.Raw);
    //             return message;
    //         }

    //         if (data is HandshakeResponse response)
    //         {
    //             var message = new HandshakeResponseMessage(response);

    //             Console.WriteLine("c!");
    //             return message;
    //         }

    //         throw new Exception("This should not be happened. Data is not a binary, but could not convert to the message.");
    //     }
    // }

    // public class ServerMessageSender : ProtocolSender
    // {
    //     public ServerMessageSender(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     protected override ISocketControllerMessage OnDataReceived(IData data)
    //     {
    //         if (data is IBinaryData binaryData)
    //         {
    //             var message = new RawDataMessage(binaryData.Raw);
    //             return message;
    //         }

    //         if (data is Handshake handshake)
    //         {
    //             var message = new HandshakeMessage(handshake);
    //             return message;
    //         }

    //         if (data is HandshakeResponse handshakeResponse)
    //         {
    //             var message = new HandshakeResponseMessage(handshakeResponse);
    //             return message;
    //         }

    //         throw new Exception("This should not be happened. Data is not a binary, but could not convert to the message.");
    //     }
    // }
}