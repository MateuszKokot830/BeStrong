using Domain.Aggregates;

namespace Domain.Tests.Aggregates
{
    public class UserTests
    {
        [Fact]
        public void Age_IsComputedFromDateOfBirth()
        {
            var user = new User { DateOfBirth = DateTime.Today.AddYears(-30) };

            Assert.Equal(30, user.Age);
        }

        [Fact]
        public void WorkoutSince_WhenDateOfWorkoutStartIsNull_ReturnsNull()
        {
            var user = new User { DateOfWorkoutStart = null };

            Assert.Null(user.WorkoutSince);
        }

        [Fact]
        public void WorkoutSince_WhenDateOfWorkoutStartIsSet_ReturnsNonEmptyString()
        {
            var user = new User { DateOfWorkoutStart = DateTime.Today.AddDays(-40) };

            Assert.Equal("1 months 9 days ", user.WorkoutSince);
        }
    }
}
