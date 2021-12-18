using System;

namespace Min.MySqlProxyServer
{
    public interface IPacketService
    {
        IObservable<IBinaryData> ToBinaryData(IObservable<IData> dataStream);

        IObservable<IData> FromBinaryData(IObservable<IBinaryData> dataStream);
    }
}
