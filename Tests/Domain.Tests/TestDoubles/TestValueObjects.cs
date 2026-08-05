using Domain.Models;

namespace Domain.Tests.TestDoubles
{
    internal sealed class PointValueObject(int x, int y) : ValueObject
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }

    internal sealed class OtherValueObject(int x) : ValueObject
    {
        public int X { get; } = x;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return X;
        }
    }

    internal sealed class FirstIntEntity : Entity<int>
    {
    }

    internal sealed class SecondIntEntity : Entity<int>
    {
    }
}
