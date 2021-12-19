using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class Controller
    {
        private readonly PayloadSenderService sender;
        private readonly PayloadReceiverService receiver;

        public Controller(
            PayloadSenderFactory senderFactory,
            ProtocolReceiverFactory receiverFactory)
        {
            this.sender = senderFactory.Create();
            this.receiver = receiverFactory.Create();
        }

        public void SetMessageStream(IObservable<ISocketControllerMessage> messageStream)
        {
            var protocolStream = messageStream.Select<ISocketControllerMessage, IData>(message =>
            {
                if (message is RawDataMessage rawDataMessage)
                {
                    return new BinaryData(rawDataMessage.Raw);
                }

                if (message is IProtocolMessage protocolMessage)
                {
                    return protocolMessage.Protocol;
                }

                throw new Exception("This should not be happened. Message is not RawDataMessage or IProtocolMessage.");
            });

            this.receiver.GetDataStream(protocolStream);
        }
    }
}