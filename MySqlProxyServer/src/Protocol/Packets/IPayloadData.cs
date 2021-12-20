using System.Collections.Generic;

namespace Min.MySqlProxyServer.Protocol
{
    public interface IPayloadData : IData
    {
        int InitialSequenceId { get; }

        IEnumerable<byte[]> Payloads { get; }
    }
}