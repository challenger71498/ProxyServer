namespace ProxyServer.Protocol
{
    public class Packet
    {
        public int payloadLength;
        public int sequenceId;
        public byte[] payload;
    }
}
