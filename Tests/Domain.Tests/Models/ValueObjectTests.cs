using Domain.Tests.TestDoubles;

namespace Domain.Tests.Models
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equals_WithSameComponentsInSameOrder_ReturnsTrue()
        {
            var a = new PointValueObject(1, 2);
            var b = new PointValueObject(1, 2);

            Assert.True(a.Equals(b));
            Assert.True(a == b);
        }

        [Fact]
        public void Equals_WithDifferentComponents_ReturnsFalse()
        {
            var a = new PointValueObject(1, 2);
            var b = new PointValueObject(2, 1);

            Assert.False(a.Equals(b));
            Assert.True(a != b);
        }

        [Fact]
        public void Equals_WithDifferentConcreteType_ReturnsFalseEvenIfComponentsMatch()
        {
            var a = new PointValueObject(1, 2);
            var b = new OtherValueObject(1);

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            var a = new PointValueObject(1, 2);

            Assert.False(a.Equals(null));
        }

        [Fact]
        public void EqualityOperator_WithBothNull_ReturnsTrue()
        {
            PointValueObject? a = null;
            PointValueObject? b = null;

            Assert.True(a == b);
        }

        [Fact]
        public void GetHashCode_ForEqualObjects_IsTheSame()
        {
            var a = new PointValueObject(1, 2);
            var b = new PointValueObject(1, 2);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_IsOrderInsensitive_UnlikeEquals()
        {
            var a = new PointValueObject(1, 2);
            var b = new PointValueObject(2, 1);

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
            Assert.NotEqual(a, b);
        }
    }
}
