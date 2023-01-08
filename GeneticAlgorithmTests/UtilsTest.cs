using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeneticAlgorithm;
using static GeneticAlgorithm.Utils;

namespace SmallestSquareTests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void SplitByteStandardInput()
        {
            byte input = 195;

            KeyValuePair<byte, byte> value = SplitByteKVP(input);

            KeyValuePair<byte, byte> expectedValue = new(12, 3);

            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void CombineBytesStandardKVPInput()
        {
            KeyValuePair<byte, byte> input = new(12, 3);

            int value = CombineBytes(input);

            int expectedValue = 195;

            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void CombineBytesStandardInput()
        {
            byte leftByte = 12;
            byte rightByte = 3;

            int value = CombineBytes(leftByte, rightByte);

            int expectedValue = 195;

            Assert.AreEqual(expectedValue, value);
        }
    }
}