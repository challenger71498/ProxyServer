namespace System.IO
{
    using System.Globalization;
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

            var number = ReadFixedInt(reader, length);

            return number;
        }

        public static string ReadFixedString(this BinaryReader reader, int length)
        {
            var binary = reader.ReadBytes(length);

            var str = Encoding.ASCII.GetString(binary);
            return str;
        }

        public static string ReadNulTerminatedString(this BinaryReader reader)
        {
            var stringBuilder = new StringBuilder();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var binary = reader.ReadByte();
                var letter = (char)binary;

                if (letter == '\0')
                {
                    return stringBuilder.ToString();
                }

                stringBuilder.Append(letter);
            }

            throw new Exception("Unexpected EOF."); // TODO: Exception handling
        }

        public static string ReadLengthEncodedString(this BinaryReader reader)
        {
            var length = reader.ReadLengthEncodedInt();
            var str = reader.ReadFixedString(length);

            return str;
        }

        public static string ReadRestOfPacketString(this BinaryReader reader)
        {
            var stringBuilder = new StringBuilder();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var binary = reader.ReadByte();
                var letter = (char)binary;

                stringBuilder.Append(letter);
            }

            return stringBuilder.ToString();
        }
    }
}
