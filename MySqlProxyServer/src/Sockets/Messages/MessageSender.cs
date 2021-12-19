using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public abstract class MessageSender
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;
        protected readonly IProtocolService protocolService;

        public MessageSender(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public IObservable<ISocketControllerMessage> GetMessageStream(IObservable<byte[]> dataStream)
        {
            var messageStream = dataStream
                .Select(binary => new BinaryData(binary))
                .Let(this.packetService.FromBinaryData)
                .Let(this.payloadService.FromPacket)
                .Let(this.protocolService.FromPayload)
                .Select(this.OnDataReceived);

            return messageStream;
        }

        protected abstract ISocketControllerMessage OnDataReceived(IData data);
    }

    public class ClientMessageSender : MessageSender
    {
        public ClientMessageSender(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
            : base(packetService, payloadService, protocolService)
        {
        }

        protected override ISocketControllerMessage OnDataReceived(IData data)
        {
            if (data is IBinaryData binaryData)
            {
                var message = new RawDataMessage(binaryData.Raw);
                return message;
            }

            if (data is HandshakeResponse response)
            {
                var message = new HandshakeResponseMessage(response);

                Console.WriteLine("c!");
                return message;
            }

            throw new Exception("This should not be happened. Data is not a binary, but could not convert to the message.");
        }
    }

    public class ServerMessageSender : MessageSender
    {
        public ServerMessageSender(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
            : base(packetService, payloadService, protocolService)
        {
        }

        protected override ISocketControllerMessage OnDataReceived(IData data)
        {
            if (data is IBinaryData binaryData)
            {
                var message = new RawDataMessage(binaryData.Raw);
                return message;
            }

            if (data is Handshake handshake)
            {
                var message = new HandshakeMessage(handshake);
                return message;
            }

            if (data is HandshakeResponse handshakeResponse)
            {
                var message = new HandshakeResponseMessage(handshakeResponse);
                return message;
            }

            throw new Exception("This should not be happened. Data is not a binary, but could not convert to the message.");
        }
    }
}