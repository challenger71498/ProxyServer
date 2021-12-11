namespace System.IO
{
    using System;
    using System.Text;
    using Min.MySqlProxyServer.Protocol;

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

        public static void WriteLengthEncodedInt(this BinaryWriter writer, int number)
        {
            var binary = DataTypeConverter.FromInt(number);

            if (number < 251)
            {
                writer.Write(binary[0]);
            }
            else if (number < (1 << 16))
            {
                writer.Write((byte)0xfc);
                writer.Write(binary[..2]);
            }
            else if (number < (1 << 24))
            {
                writer.Write((byte)0xfd);
                writer.Write(binary[..3]);
            }
            else
            {
                writer.Write((byte)0xfe);
                writer.Write(binary[..8]);
            }
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

        public static void WriteLengthEncodedString(this BinaryWriter writer, string str)
        {
            writer.WriteLengthEncodedInt(str.Length);
            writer.WriteFixedString(str, str.Length);
        }
    }
}
