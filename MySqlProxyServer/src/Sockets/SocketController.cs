// Copyright (c) Min. All rights reserved.

using System;
using System.Reactive;
using System.Threading.Tasks;

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
        }

        public IObservable<ISocketControllerMessage> WhenMessageCreated { get; private set; }

        public void SetMessageReceiveStream(IObservable<ISocketControllerMessage> messageReceiveStream)
        {
            this.receiver.GetDataStream(messageReceiveStream).Subscribe(this.OnDataReceived);
        }

        private async void OnDataReceived(byte[] data)
        {
            await this.connection.Send(data);
        }
    }
}
