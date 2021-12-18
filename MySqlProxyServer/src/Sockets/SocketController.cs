// Copyright (c) Min. All rights reserved.

using System;

namespace Min.MySqlProxyServer.Sockets
{
    public class SocketController : ISocketController
    {
        private readonly ISocketConnection connection;
        private readonly MessageSender sender;
        private readonly MessageReceiver receiver;

        public SocketController(
            ISocketConnection connection,
            MessageSender sender,
            MessageReceiver receiver)
        {
            this.connection = connection;
            this.sender = sender;
            this.receiver = receiver;

            this.WhenMessageCreated = this.sender.GetMessageStream(this.connection.WhenDataReceived);

            this.connection.WhenDisconnected.Subscribe(this.OnDisconnected);
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
            await this.connection.Send(data.Raw);
        }

        private async void OnDisconnected(bool _)
        {
            // TODO: Handle disconnected event.
            Console.WriteLine("Disconnected!");
        }
    }
}
