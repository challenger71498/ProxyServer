using System;

namespace ProxyServer.Protocol.DataType
{
    public class FixedIntegerType : FixedLengthType
    {
        public FixedIntegerType(int byteLength) : base(byteLength)
        {
            // Empty
        }

        public int Read(byte[] binary)
        {
            Array.Reverse(binary);

            var hex = Convert.ToHexString(binary);
            var number = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            return number;
        }
    }
}