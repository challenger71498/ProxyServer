namespace Min.MySqlProxyServer.Protocol
{
    public interface IPayloadData : IData
    {
        int InitialSequenceId { get; }

        byte[] Payload { get; }
    }
}