// Copyright (c) Min. All rights reserved.

using System.Collections.Generic;
using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    public class ErrorProtocol : IWritableProtocol
    {
        public int ErrorCode { get; set; }

        public byte[] ErrorMessage { get; set; }

        public byte[]? SqlState { get; set; }

        public IEnumerable<IProtocolFactory>? NextAvailableProtocolFactories { get; } = null;

        public byte[] ToPayload()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(0xff, 1);

            writer.WriteFixedInt(this.ErrorCode, 2);

            if (this.SqlState != null)
            {
                writer.WriteFixedString(System.Text.Encoding.ASCII.GetBytes("#"), 1);
                writer.WriteFixedString(this.SqlState, 5);
            }

            writer.WriteFixedString(this.ErrorMessage, this.ErrorMessage.Length);

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return binary;
        }
    }
}
