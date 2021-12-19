using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Encryption;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{

    public class ProtocolReceiver
    {
        protected readonly IPacketService packetService;
        protected readonly IPayloadService payloadService;
        protected readonly IProtocolService protocolService;
        private int id;

        public ProtocolReceiver(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService)
        {
            this.id = 1;

            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
        }

        public IObservable<IBinaryData> GetDataStream(IObservable<IData> protocolStream)
        {
            var dataStream = protocolStream
                .Do(this.SequenceIdSetter)
                .Let(protocol => this.protocolService.ToPayload(this.id, protocol))
                .Let(this.payloadService.ToPacket)
                .Let(this.packetService.ToBinaryData)
                .Catch((Exception e) =>
                {
                    Console.Error.WriteLine($"Error on streaming message to data: {e.GetType()} {e.Message}");
                    Console.Error.WriteLine(e.StackTrace);
                    return Observable.Empty<IBinaryData>();
                })
                .Repeat();

            return dataStream;
        }

        private void SequenceIdSetter(IData data)
        {
            if (data.GetType() != typeof(ICommandProtocol))
            {
                return;
            }

            this.id = 0;
        }
    }

    // public class ClientMessageReceiver : ProtocolReceiver
    // {
    //     private readonly AuthService authService;

    //     public ClientMessageReceiver(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService,
    //         AuthService authService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //         this.authService = authService;
    //     }

    //     protected override IData OnMessageReceived(ISocketControllerMessage message)
    //     {
    //         if (message is RawDataMessage rawMessage)
    //         {
    //             var data = new BinaryData(rawMessage.Raw);
    //             return data;
    //         }

    //         if (message is HandshakeResponseMessage responseMessage)
    //         {
    //             var protocol = responseMessage.Protocol;
    //             return protocol;
    //         }

    //         if (message is AuthSwitchResponseMessage authSwitchResponseMessage)
    //         {
    //             var protocol = authSwitchResponseMessage.Protocol;
    //             var authData = this.authService.GetAuthData(HashAlgorithmType.SHA1, "foobar", "?");
    //         }

    //         throw new Exception("This should not be happened. Message is not a raw type, but could not convert to protocol.");
    //     }
    // }

    // public class ServerMessageReceiver : ProtocolReceiver
    // {
    //     public ServerMessageReceiver(
    //         IPacketService packetService,
    //         IPayloadService payloadService,
    //         IProtocolService protocolService)
    //         : base(packetService, payloadService, protocolService)
    //     {
    //     }

    //     protected override IData OnMessageReceived(ISocketControllerMessage message)
    //     {
    //         if (message is RawDataMessage rawMessage)
    //         {
    //             var data = new BinaryData(rawMessage.Raw);
    //             return data;
    //         }

    //         if (message is HandshakeResponseMessage responseMessage)
    //         {
    //             var protocol = responseMessage.Protocol;
    //             return protocol;
    //         }

    //         throw new Exception("This should not be happened. Message is not a raw type, but could not convert to protocol.");
    //     }
    // }
}