using System.IO;

namespace Min.MySqlProxyServer.Protocol
{
    /// <summary>
    /// <see cref="Packet"/> describes MySQL base packet.
    /// </summary>
    public class Packet : BasePacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="payloadLength">Length of the payload binary.</param>
        /// <param name="sequenceId">Sequence ID of the packet.</param>
        /// <param name="payload">Binary payload.</param>
        public Packet(int payloadLength, int sequenceId, byte[] payload)
        {
            this.PayloadLength = payloadLength;
            this.SequenceId = sequenceId;
            this.Payload = payload;
        }
    }
}
