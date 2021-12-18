namespace Min.MySqlProxyServer
{
    public interface IBinaryData : IData
    {
        byte[] Raw { get; }
    }

    public class BinaryData : IBinaryData
    {
        public BinaryData(byte[] data)
        {
            this.Raw = data;
        }

        public byte[] Raw { get; }
    }
}