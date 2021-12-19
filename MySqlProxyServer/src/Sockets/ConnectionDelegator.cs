// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive.Linq;

namespace Min.MySqlProxyServer.Sockets
{
    public class ConnectionDelegator : IConnectionDelegator
    {
        private readonly ISocketConnection counterConnection;
        private readonly MessageSender sender;
        private readonly MessageReceiver receiver;

        public ConnectionDelegator(
            ISocketConnection counterConnection,
            MessageSender sender,
            MessageReceiver receiver)
        {
            this.counterConnection = counterConnection;
            this.sender = sender;
            this.receiver = receiver;

            this.WhenMessageCreated = this.sender.GetMessageStream(this.counterConnection.WhenDataReceived);
            this.counterConnection.WhenDisconnected.Subscribe(this.OnDisconnected);
        }

        /// <inheritdoc/>
        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }

        /// <inheritdoc/>
        public void SetMessageReceiveStream(IObservable<ISocketControllerMessage> messageReceiveStream)
        {
            // TODO: Life cycle management needed.
            var dataStream = this.receiver.GetDataStream(messageReceiveStream);
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
