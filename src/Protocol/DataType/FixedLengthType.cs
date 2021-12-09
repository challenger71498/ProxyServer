namespace ProxyServer.Protocol.DataType
{
    public abstract class FixedLengthType : IDataType
    {
        public int ByteLength { get; }

        public FixedLengthType(int byteLength)
        {
            ByteLength = byteLength;
        }
    }
}