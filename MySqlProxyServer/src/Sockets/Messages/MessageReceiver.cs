using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{

    public class MessageReceiver
    {
        private readonly IPacketService packetService;
        private readonly IPayloadService payloadService;
        private readonly IProtocolService protocolService;
        private readonly IMessageService messageService;

        private int id;

        public MessageReceiver(
            IPacketService packetService,
            IPayloadService payloadService,
            IProtocolService protocolService,
            IMessageService messageService)
        {
            this.id = 0;

            this.packetService = packetService;
            this.payloadService = payloadService;
            this.protocolService = protocolService;
            this.messageService = messageService;
        }

        public IObservable<IBinaryData> GetDataStream(IObservable<ISocketControllerMessage> messageStream)
        {
            var dataStream = messageStream
                .Let(this.messageService.ToProtocol)
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
}