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
        private readonly PayloadSenderService senderService;
        private readonly PayloadReceiverService receiverService;

        public ConnectionDelegator(
            ISocketConnection counterConnection,
            PayloadSenderService sender,
            PayloadReceiverService receiver)
        {
            this.counterConnection = counterConnection;
            this.senderService = sender;
            this.receiverService = receiver;

            this.WhenDataCreated = this.senderService.GetPayloadStream(this.counterConnection.WhenDataReceived);
            this.counterConnection.WhenDisconnected.Subscribe(this.OnDisconnected);
        }

        /// <inheritdoc/>
        public IObservable<IData> WhenDataCreated { get; private set; }

        /// <inheritdoc/>
        public void SetDataReceiveStream(IObservable<IData> dataStream)
        {
            // TODO: Life cycle management needed.
            var receivedStream = this.receiverService.GetDataStream(dataStream);
            receivedStream.Subscribe(this.OnDataReceived);
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
