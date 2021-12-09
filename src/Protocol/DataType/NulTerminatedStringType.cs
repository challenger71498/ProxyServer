using System;
using System.IO;
using System.Text;

namespace ProxyServer.Protocol.DataType
{
    public class NulTerminatedStringType : IDataType
    {
        public string Read(byte[] binary)
        {
            var stream = new MemoryStream(binary);

            var buffer = new byte[1];
            var stringBuilder = new StringBuilder();

            while (stream.Position < stream.Length)
            {
                stream.Read(buffer);
                var letter = Encoding.ASCII.GetChars(buffer)[0];

                if (letter == '\0')
                {
                    return stringBuilder.ToString();
                }

                stringBuilder.Append(letter);
            }

            throw new Exception("Stream buffer ended without null character."); // TODO: Exception handling
        }
    }
}