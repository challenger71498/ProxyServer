using System.Collections.Generic;

namespace Min.MySqlProxyServer.Protocol
{
    public class PayloadData : IPayloadData
    {
        public PayloadData(int initialSequenceId, IEnumerable<byte[]> payload)
        {
            this.InitialSequenceId = initialSequenceId;
            this.Payloads = payload;
        }

        public int InitialSequenceId { get; set; }

        public IEnumerable<byte[]> Payloads { get; set; }
    }
}