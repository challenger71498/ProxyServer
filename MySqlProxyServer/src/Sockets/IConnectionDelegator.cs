// Copyright (c) Min. All rights reserved.

using System;

namespace Min.MySqlProxyServer.Sockets
{
    public interface IConnectionDelegator
    {
        IObservable<IData> WhenDataCreated { get; }

        void SetDataReceiveStream(IObservable<IData> dataReceiveStream);
    }
}
