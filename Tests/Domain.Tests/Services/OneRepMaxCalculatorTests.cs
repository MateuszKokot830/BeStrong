using Domain.Services;

namespace Domain.Tests.Services
{
    public class OneRepMaxCalculatorTests
    {
        [Theory]
        [InlineData(100, 1, 100)]
        [InlineData(100, 5, 113)]
        [InlineData(100, 10, 134)]
        [InlineData(60, 8, 75)]
        public void Calculate_ForTypicalWeightAndReps_ReturnsExpectedOneRepMax(decimal weight, int reps, int expected)
        {
            var result = OneRepMaxCalculator.Calculate(weight, reps);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Calculate_WithOneRep_ReturnsWeightItself()
        {
            var result = OneRepMaxCalculator.Calculate(100, 1);

            Assert.Equal(100, result);
        }

        [Fact]
        public void Calculate_WithZeroReps_DoesNotThrow()
        {
            var result = OneRepMaxCalculator.Calculate(100, 0);

            Assert.Equal(98, result);
        }

        [Fact]
        public void Calculate_WithZeroWeight_ReturnsZero()
        {
            var result = OneRepMaxCalculator.Calculate(0, 5);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Calculate_NearRepsThirtySeven_DenominatorApproachesZero_ResultBecomesUnreasonablyLarge()
        {
            var result = OneRepMaxCalculator.Calculate(100, 36);

            Assert.True(result > 1000);
        }

        [Fact]
        public void Calculate_AtRepsWhereDenominatorCrossesZero_ResultFlipsToHugeNegative()
        {
            var result = OneRepMaxCalculator.Calculate(100, 37);

            Assert.True(result < -1000);
        }

        [Fact]
        public void Calculate_PastRepsThirtySeven_DenominatorGoesNegative_ResultBecomesNegative()
        {
            var result = OneRepMaxCalculator.Calculate(100, 38);

            Assert.True(result < 0);
        }
    }
}
