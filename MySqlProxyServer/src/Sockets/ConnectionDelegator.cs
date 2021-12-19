// Copyright (c) Min. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Min.MySqlProxyServer.Protocol;

namespace Min.MySqlProxyServer.Sockets
{
    public class ConnectionDelegator : IConnectionDelegator
    {
        private readonly ISocketConnection counterConnection;
        private readonly ProtocolSender sender;
        private readonly ProtocolReceiver receiver;

        private readonly List<IData> dataList;
        private readonly IObservable<IData> protocolSendStream;

        public ConnectionDelegator(
            ISocketConnection counterConnection,
            ProtocolSender sender,
            ProtocolReceiver receiver)
        {
            this.counterConnection = counterConnection;
            this.sender = sender;
            this.receiver = receiver;

            var protocolReceivedStream = this.sender.GetProtocolStream(this.counterConnection.WhenDataReceived);

            protocolReceivedStream.Subscribe(data =>
            {
                if (sendBack)
                {
                    this.protocolSendStream.Append(newData);
                    return;
                }

                // Send a message?
                messageSendStream.Append(message);
            });

            this.counterConnection.WhenDisconnected.Subscribe(this.OnDisconnected);
        }

        /// <inheritdoc/>
        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }

        /// <inheritdoc/>
        public void SetMessageReceiveStream(IObservable<ISocketControllerMessage> messageReceiveStream)
        {
            var protocolStream = messageReceiveStream.Select<ISocketControllerMessage, IData>(message =>
            {
                if (message is RawDataMessage rawDataMessage)
                {
                    return new BinaryData(rawDataMessage.Raw);
                }

                if (message is IProtocolMessage protocolMessage)
                {
                    // Do something...
                    return protocolMessage.Protocol;
                }

                throw new Exception("This should not be happened. Message is not RawDataMessage or IProtocolMessage.");
            });



            // TODO: Life cycle management needed.
            var dataStream = this.receiver.GetDataStream(this.protocolSendStream);
            dataStream.Subscribe(this.OnDataReceived);
        }

        private async void OnDataReceived(IBinaryData data)
        {
            await this.counterConnection.Send(data.Raw);
        }

        private async void OnDisconnected(bool _)
        {
            // TODO: Handle disconnected event.
            Console.WriteLine("Disconnected!");
        }
    }
}
