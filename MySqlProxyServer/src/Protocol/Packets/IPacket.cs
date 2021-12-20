using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    /// <summary>
    /// <see cref="IPacket"/> is an interface for MySQL base packet.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// Gets or sets the length of the payload binary.
        /// </summary>
        /// <value>Payload binary length as int.</value>
        int PayloadLength { get; set; }

        /// <summary>
        /// Gets or sets the sequence ID of the packet.
        /// </summary>
        /// <value>Sequence ID as int.</value>
        int SequenceId { get; set; }

        /// <summary>
        /// Gets or sets the payload binary of the packet.
        /// </summary>
        /// <value>Payload as byte array.</value>
        byte[] Payload { get; set; }

        IBinaryData ToBinary();
    }

    public abstract class BasePacket : IPacket
    {
        /// <inheritdoc/>
        public int PayloadLength { get; set; }

        /// <inheritdoc/>
        public int SequenceId { get; set; }

        /// <inheritdoc/>
        public byte[] Payload { get; set; }

        /// <inheritdoc/>
        public IBinaryData ToBinary()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            writer.WriteFixedInt(this.PayloadLength, 3);
            writer.WriteFixedInt(this.SequenceId, 1);
            writer.Write(this.Payload);

            // System.Console.WriteLine($"STREAM_LEN: {stream.Length}");

            var buffer = stream.GetBuffer();
            var binary = buffer[..(int)stream.Length];

            return new BinaryData(binary);
        }
    }
}
