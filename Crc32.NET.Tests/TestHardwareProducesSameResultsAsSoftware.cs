using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Crc32.Tests
{
    [TestFixture]
    public class TestHardwareProducesSameResultsAsSoftware
    {
        [TestCase("just some simple test")]
        [TestCase("just some simple test, another example")]
        [TestCase("just some simple test, another examplea")]
        [TestCase("just some simple test, another exampleab")]
        [TestCase("just some simple test, another exampleabcde")]
        public void Test_StringsAsUtf8_ReturnSameHashes(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);

            var sw = Force.Crc32.Crc32CAlgorithm.Compute(bytes);
            var hw = Force.Crc32.Crc32CAlgorithmHW.Compute(bytes);

            Assert.AreEqual(sw, hw);
        }

        [TestCase("just some simple test", 3, 2)]
        [TestCase("just some simple test, another example", 1, 15)]
        [TestCase("just some simple test, another examplea", 2, 3)]
        [TestCase("just some simple test, another exampleab", 6, 15)]
        [TestCase("just some simple test, another exampleabcde", 0, 8)]
        public void Test_StringsAsUtf8WithOffsets_ReturnSameHashes(string s, int offset, int length)
        {
            var bytes = Encoding.UTF8.GetBytes(s);

            var sw = Force.Crc32.Crc32CAlgorithm.Compute(bytes, offset, length);
            var hw = Force.Crc32.Crc32CAlgorithmHW.Compute(bytes, offset, length);

            Assert.AreEqual(sw, hw);
        }
    }
}
