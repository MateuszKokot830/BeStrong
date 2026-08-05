using Domain.Tests.TestDoubles;

namespace Domain.Tests.Models
{
    public class EntityTests
    {
        [Fact]
        public void Equals_WithSameIdAndSameType_ReturnsTrue()
        {
            var a = new FirstIntEntity { Id = 1 };
            var b = new FirstIntEntity { Id = 1 };

            Assert.True(a.Equals(b));
            Assert.True(a == b);
        }

        [Fact]
        public void Equals_WithDifferentId_ReturnsFalse()
        {
            var a = new FirstIntEntity { Id = 1 };
            var b = new FirstIntEntity { Id = 2 };

            Assert.False(a.Equals(b));
            Assert.True(a != b);
        }

        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            var a = new FirstIntEntity { Id = 1 };

            Assert.False(a.Equals(null));
        }

        [Fact]
        public void EqualityOperator_WithBothNull_ReturnsTrue()
        {
            FirstIntEntity? a = null;
            FirstIntEntity? b = null;

            Assert.True(a == b);
        }

        [Fact]
        public void Equals_AcrossDifferentEntityTypesWithSameId_ReturnsTrueDueToLooseTypeCheck()
        {
            var post = new FirstIntEntity { Id = 1 };
            var workout = new SecondIntEntity { Id = 1 };

            Assert.True(post.Equals(workout));
        }

        [Fact]
        public void GetHashCode_ForEqualIds_IsTheSame()
        {
            var a = new FirstIntEntity { Id = 5 };
            var b = new FirstIntEntity { Id = 5 };

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
    }
}
