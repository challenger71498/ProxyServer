using System;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Sockets;

namespace Min.MySqlProxyServer
{
    public class Controller
    {
        private readonly IObservable<IData> protocolStreamFromCounterDelegator;
        private readonly IObservable<byte[]> dataStreamFromCounter;
        private readonly IObservable<ISocketControllerMessage> messageStreamToCounterDelegator;
        private readonly IObservable<IBinaryData> dataStreamToCounter;

        public Controller(
            IObservable<ISocketControllerMessage> messageStreamFromCounterDelegator,
            IObservable<byte[]> dataStreamFromCounter,
            IObservable<ISocketControllerMessage> messageStreamToCounterDelegator,
            IObservable<IBinaryData> dataStreamToCounter
        )
        {
            this.messageStreamFromCounterDelegator = messageStreamFromCounterDelegator;
            this.dataStreamFromCounter = dataStreamFromCounter;
            this.messageStreamToCounterDelegator = messageStreamToCounterDelegator;
            this.dataStreamToCounter = dataStreamToCounter;
        }

        public void OnMessageFromCounterDelegatorReceived(ISocketControllerMessage message)
        {
            // blah blah blah...
            var receiver = new MessageReceiver();

            receiver.GetDataStream();

            // Send to data stream.
            dataStreamFromCounter.Append()
        }
    }
}