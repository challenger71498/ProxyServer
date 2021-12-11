namespace Min.MySqlProxyServer.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class BaseBinaryIOTest
    {
        public byte[] fixedIntBinary;
        public int fixedInt;

        public byte[] lengthEncodedIntBinary;
        public int lengthEncodedInt;

        public byte[] fixedStringBinary;
        public string fixedString;

        public byte[] nulTerminatedStringBinary;
        public string nulTerminatedString;
        public byte[] nulTerminatledStringAnswerBinary;

        [SetUp]
        public void Setup()
        {
            var fixedIntHex = "4A000000";
            this.fixedIntBinary = Convert.FromHexString(fixedIntHex);
            this.fixedInt = 74;

            var lengthEncodedIntHex = "FCFB00";
            this.lengthEncodedIntBinary = Convert.FromHexString(lengthEncodedIntHex);
            this.lengthEncodedInt = 251;

            var fixedStringHex = "48656C6C6F2C20776F726C6421";
            this.fixedStringBinary = Convert.FromHexString(fixedStringHex);
            this.fixedString = "Hello, world!";

            var nulTerminatedStringHex = "48656C6C6F2C20776F726C64210048656C6C6F2C20776F726C6421";
            this.nulTerminatedStringBinary = Convert.FromHexString(nulTerminatedStringHex);
            this.nulTerminatedString = "Hello, world!";

            var nulTerminatedStringAnswerHex = "48656C6C6F2C20776F726C642100";
            this.nulTerminatledStringAnswerBinary = Convert.FromHexString(nulTerminatedStringAnswerHex);
        }
    }
}