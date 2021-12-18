// Copyright (c) Min. All rights reserved.

using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public struct ErrorProtocol : IWritableProtocol
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string? SqlState { get; set; }

        public byte[] ToPayload()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(0xff, 1);

            writer.WriteFixedInt(this.ErrorCode, 2);

            if (this.SqlState != null)
            {
                writer.WriteFixedString("#", 1);
                writer.WriteFixedString(this.SqlState, 5);
            }

            writer.WriteFixedString(this.ErrorMessage, this.ErrorMessage.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }
    }
}
