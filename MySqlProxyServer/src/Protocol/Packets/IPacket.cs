namespace MySql.ProxyServer.Protocol
{
    interface IPacket
    {
        int PayloadLength { get; }
        int SequenceId { get; }
        byte[] Payload { get; }
    }
}