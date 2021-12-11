namespace Min.MySqlProxyServer.Tests
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class BinaryReaderExtensionTest : BaseBinaryIOTest
    {
        [Test]
        public void ReadFixedIntTest()
        {
            using var stream = new MemoryStream(this.fixedIntBinary);
            using var reader = new BinaryReader(stream);

            var ans = reader.ReadFixedInt(this.fixedIntBinary.Length);

            Assert.IsTrue(ans == this.fixedInt, $"Answer {ans} should be {this.fixedInt}");
        }

        [Test]
        public void ReadLengthEncodedIntTest()
        {
            using var stream = new MemoryStream(this.lengthEncodedIntBinary);
            using var reader = new BinaryReader(stream);

            var ans = reader.ReadLengthEncodedInt();

            Assert.IsTrue(ans == this.lengthEncodedInt, $"Answer {ans} should be {this.lengthEncodedInt}");
        }

        [Test]
        public void ReadFixedStringTest()
        {
            using var stream = new MemoryStream(this.fixedStringBinary);
            using var reader = new BinaryReader(stream);

            var ans = reader.ReadFixedString(this.fixedStringBinary.Length);

            Assert.IsTrue(ans == this.fixedString, $"Answer {ans} should be {this.fixedString}");
        }

        [Test]
        public void ReadNulTerminatedStringTest()
        {
            using var stream = new MemoryStream(this.nulTerminatedStringBinary);
            using var reader = new BinaryReader(stream);

            var ans = reader.ReadNulTerminatedString();

            Assert.IsTrue(ans == this.nulTerminatedString, $"Answer {ans} should be {this.nulTerminatedString}");
        }
    }
}