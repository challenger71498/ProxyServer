using System.Text;

namespace ProxyServer.Protocol.DataType
{
    public class FixedStringType : FixedLengthType
    {
        public FixedStringType(int byteLength) : base(byteLength)
        {
            // Empty
        }

        public string Read(byte[] binary)
        {
            var str = Encoding.ASCII.GetString(binary);
            return str;
        }
    }
}