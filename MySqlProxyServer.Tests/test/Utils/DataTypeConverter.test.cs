namespace MySql.ProxyServer.Tests
{
    using System;
    using System.Text;
    using MySql.ProxyServer.Protocol;
    using NUnit.Framework;

    [TestFixture]
    public class DataTypeConverterTest
    {
        private int number;
        private string numberHex;

        private string str;
        private string strHex;

        [SetUp]
        public void Setup()
        {
            number = 1253324;
            numberHex = "CC1F1300";

            str = "Hello, world!";
            strHex = "48656C6C6F2C20776F726C6421";
        }

        [Test]
        public void BinaryToIntTest()
        {
            var binary = Convert.FromHexString(numberHex);
            var ans = DataTypeConverter.ToInt(binary);

            Assert.IsTrue(ans == number, $"Answer {ans} should be {number}");
        }

        [Test]
        public void BinaryToStringTest()
        {
            var binary = Convert.FromHexString(strHex);
            var ans = DataTypeConverter.ToString(binary, Encoding.ASCII);

            Assert.IsTrue(ans == str, $"Answer {ans} should be {str}");
        }

        [Test]
        public void IntToBinaryTest()
        {
            var binary = DataTypeConverter.FromInt(number);
            var hex = Convert.ToHexString(binary);

            Assert.IsTrue(hex == numberHex, $"Answer {hex} should be {numberHex}");
        }

        [Test]
        public void StringToBinaryTest()
        {
            var binary = DataTypeConverter.FromString(str, Encoding.ASCII);
            var hex = Convert.ToHexString(binary);

            Assert.IsTrue(hex == strHex, $"Answer {hex} should be {strHex}");
        }
    }
}