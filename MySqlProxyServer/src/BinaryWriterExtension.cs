

namespace System.IO
{
    using System;
    using System.IO;
    using System.Text;
    using MySql.ProxyServer.Protocol;

    public static class BinaryWriterExtension
    {
        public static void WriteFixedInt(this BinaryWriter writer, int number, int length)
        {
            var binary = DataTypeConverter.FromInt(number);

            for (var i = 0; i < length; ++i)
            {
                writer.Write(binary[i]);
            }
        }

        public static void WriteLengthEncodedInt(this BinaryWriter writer, int number, int numberLength)
        {
            var binary = DataTypeConverter.FromInt(number);

            if (numberLength == 1)
            {
                writer.Write(binary[0]);
                return;
            }

            byte? flag = numberLength switch
            {
                2 => 0xfc,
                3 => 0xfd,
                8 => 0xfe,
                _ => throw new ArgumentException("Length is invalid. Should be one of: 1, 2, 3, 8.")
            };

            writer.Write((byte)flag);

            writer.Write(binary[..numberLength]);
        }

        public static void WriteFixedString(this BinaryWriter writer, string str, int length)
        {
            var binary = DataTypeConverter.FromString(str, Encoding.ASCII);

            for (var i = 0; i < length; ++i)
            {
                writer.Write(binary[i]);
            }
        }

        public static void WriteNulTerminatedString(this BinaryWriter writer, string str)
        {
            writer.WriteFixedString(str, str.Length);
            writer.Write('\0');
        }
    }
}

