using ModuleHelper.Utility;
using NUnit.Framework;

namespace ModuleHelper.Tests
{
    [TestFixture]
    public class MathUtilsTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(-3, 3, 0)]
        [TestCase(-10, 2, 0)]
        [TestCase(-8, 3, 1)]
        [TestCase(-2, 4, 2)]
        public void Modulo_NegativeNumberAs1stParam_PositiveResult(int x, int m, int expectedResult)
        {
            Assert.AreEqual(MathMusicalUtilities.Modulo(x, m), expectedResult);
        }

        [TestCase(8, 4, 0)]
        [TestCase(9, 4, 1)]
        [TestCase(0, 3, 0)]
        [TestCase(1, 7, 1)]
        public void Modulo_PositiveNumberAs1stParam_PositiveResult(int x, int m, int expectedResult)
        {
            Assert.AreEqual(MathMusicalUtilities.Modulo(x, m), expectedResult);
        }
    }
}
