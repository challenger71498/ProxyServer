namespace Min.MySqlProxyServer.Protocol
{
    public interface IPayloadData
    {
        int InitialSequenceId { get; }

        byte[] Payload { get; }
    }
}