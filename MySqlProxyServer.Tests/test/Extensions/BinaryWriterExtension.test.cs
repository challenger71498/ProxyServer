namespace Min.MySqlProxyServer.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class BinaryWriterExtensionTest : BaseBinaryIOTest
    {
        [Test]
        public void WriteFixedIntTest()
        {
            var output = new byte[this.fixedIntBinary.Length];

            using (var stream = new MemoryStream(output))
            {
                using var writer = new BinaryWriter(stream);
                writer.WriteFixedInt(this.fixedInt, this.fixedIntBinary.Length);

                // Console.WriteLine(stream.GetBuffer().Length);
                Console.WriteLine(output.Length);
            }

            var res = output.SequenceEqual(this.fixedIntBinary);

            Assert.IsTrue(res);
        }

        [Test]
        public void WriteLengthEncodedIntTest()
        {
            var output = new byte[this.lengthEncodedIntBinary.Length];

            using (var stream = new MemoryStream(output))
            {
                using var writer = new BinaryWriter(stream);
                writer.WriteLengthEncodedInt(this.lengthEncodedInt);
            }

            var res = output.SequenceEqual(this.lengthEncodedIntBinary);

            Assert.IsTrue(res, $"Answer {Convert.ToHexString(output)} is not expected.");
        }

        [Test]
        public void WriteFixedStringTest()
        {
            var output = new byte[this.fixedStringBinary.Length];

            using (var stream = new MemoryStream(output))
            {
                using var writer = new BinaryWriter(stream);
                writer.WriteFixedString(this.fixedString, this.fixedStringBinary.Length);
            }

            var res = output.SequenceEqual(this.fixedStringBinary);

            Assert.IsTrue(res, $"Answer {Convert.ToHexString(output)} is not expected.");
        }

        [Test]
        public void WriteNulTerminatedStringTest()
        {
            var output = new byte[this.nulTerminatledStringAnswerBinary.Length];

            using (var stream = new MemoryStream(output))
            {
                using var writer = new BinaryWriter(stream);
                writer.WriteNulTerminatedString(this.nulTerminatedString);
            }

            var res = output.SequenceEqual(this.nulTerminatledStringAnswerBinary);

            Assert.IsTrue(res, $"Answer {Convert.ToHexString(output)} is not expected.");
        }
    }
}