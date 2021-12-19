// Copyright (c) Min. All rights reserved.

using System;

namespace Min.MySqlProxyServer.Sockets
{
    public interface IConnectionDelegator
    {
        IObservable<ISocketControllerMessage> WhenMessageCreated { get; }

        void SetMessageReceiveStream(IObservable<ISocketControllerMessage> messageReceiveStream);
    }
}
