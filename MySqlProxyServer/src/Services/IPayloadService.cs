using System;

namespace Min.MySqlProxyServer
{
    public interface IPayloadService
    {
        IObservable<IData> ToPacket(IObservable<IData> dataStream);

        IObservable<IData> FromPacket(IObservable<IData> packetStream);
    }
}