namespace Min.MySqlProxyServer.Protocol
{
    public class PayloadData : IPayloadData
    {
        public PayloadData(int initialSequenceId, byte[] payload)
        {
            this.InitialSequenceId = initialSequenceId;
            this.Payload = payload;
        }

        public int InitialSequenceId { get; set; }

        public byte[] Payload { get; set; }
    }
}