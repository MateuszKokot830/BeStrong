using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects
{
    public class MeasurementsTests
    {
        [Fact]
        public void Equals_WithIdenticalValues_ReturnsTrue()
        {
            var a = new Measurements(180, 80m, 100m, 110m, 35m, 85m, 95m, 55m);
            var b = new Measurements(180, 80m, 100m, 110m, 35m, 85m, 95m, 55m);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_WithDifferentSingleField_ReturnsFalse()
        {
            var a = new Measurements(180, 80m, 100m, 110m, 35m, 85m, 95m, 55m);
            var b = new Measurements(180, 81m, 100m, 110m, 35m, 85m, 95m, 55m);

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_TreatsNullAndZeroAsEquivalent()
        {
            var withNulls = new Measurements(null, null, null, null, null, null, null, null);
            var withZeros = new Measurements(0, 0m, 0m, 0m, 0m, 0m, 0m, 0m);

            Assert.True(withNulls.Equals(withZeros));
        }

        [Fact]
        public void DefaultConstructor_ProducesAllNullFields()
        {
            var measurements = new Measurements();

            Assert.Null(measurements.Height);
            Assert.Null(measurements.Weight);
            Assert.Null(measurements.Chest);
        }
    }
}
