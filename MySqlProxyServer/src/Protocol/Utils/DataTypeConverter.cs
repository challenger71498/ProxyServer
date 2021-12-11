// Copyright (c) Min. All rights reserved.

using System;
using System.Text;

namespace Min.MySqlProxyServer.Protocol
{
    public class DataTypeConverter
    {
        public static int ToInt(byte[] binary)
        {
            Array.Reverse(binary);

            var hex = Convert.ToHexString(binary);
            var number = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

            return number;
        }

        public static string ToString(byte[] binary, Encoding encoding)
        {
            var str = encoding.GetString(binary);
            return str;
        }

        public static byte[] FromInt(int number)
        {
            // Little endian.
            var binary = BitConverter.GetBytes(number);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(binary);
            }

            return binary;
        }

        public static byte[] FromString(string str, Encoding encoding)
        {
            var binary = encoding.GetBytes(str);

            return binary;
        }
    }
}
