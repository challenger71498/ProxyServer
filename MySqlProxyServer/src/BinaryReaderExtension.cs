namespace System.IO
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class BinaryReaderExtension
    {
        public static int ReadFixedInt(this BinaryReader reader, int length)
        {
            var binary = reader.ReadBytes(length);

            Array.Reverse(binary);

            var hex = Convert.ToHexString(binary);
            var number = int.Parse(hex, NumberStyles.HexNumber);

            return number;
        }

        public static int ReadLengthEncodedInt(this BinaryReader reader)
        {
            var flag = reader.ReadByte();

            int length = flag switch
            {
                0xfc => 2,
                0xfd => 3,
                0xfe => 8,
                _ => 1
            };

            if (length == 1)
            {
                return flag;
            }

            var number = ReadFixedInt(reader, length);

            return number;
        }

        public static byte[] ReadFixedString(this BinaryReader reader, int length)
        {
            return reader.ReadBytes(length);
        }

        public static byte[] ReadNulTerminatedString(this BinaryReader reader)
        {
            var bytes = new List<byte>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var binary = reader.ReadByte();
                var letter = (char)binary;

                if (letter == '\0')
                {
                    return bytes.ToArray();
                }

                bytes.Add(binary);
            }

            // var stringBuilder = new StringBuilder();

            // while (reader.BaseStream.Position < reader.BaseStream.Length)
            // {
            //     var binary = reader.ReadByte();
            //     var letter = (char)binary;

            //     if (letter == '\0')
            //     {
            //         return stringBuilder.ToString();
            //     }

            //     stringBuilder.Append(letter);
            // }

            throw new Exception("Unexpected EOF."); // TODO: Exception handling
        }

        public static byte[] ReadLengthEncodedString(this BinaryReader reader)
        {
            var length = reader.ReadLengthEncodedInt();
            return reader.ReadFixedString(length);
        }

        public static byte[] ReadRestOfPacketString(this BinaryReader reader)
        {
            var bytes = new List<byte>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var binary = reader.ReadByte();
                bytes.Add(binary);
            }

            return bytes.ToArray();

            // var stringBuilder = new StringBuilder();

            // while (reader.BaseStream.Position < reader.BaseStream.Length)
            // {
            //     var binary = reader.ReadByte();
            //     var letter = (char)binary;

            //     stringBuilder.Append(letter);
            // }

            // return stringBuilder.ToString();
        }
    }
}
