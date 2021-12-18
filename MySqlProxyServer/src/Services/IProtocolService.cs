using System;

namespace Min.MySqlProxyServer
{
    public interface IProtocolService
    {
        IObservable<IData> ToPayload(int initialSequenceId, IObservable<IData> dataStream);

        IObservable<IData> FromPayload(IObservable<IData> dataStream);
    }
}
