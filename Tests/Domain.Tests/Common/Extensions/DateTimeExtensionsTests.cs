using Domain.Common.Extensions;

namespace Domain.Tests.Common.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void GetAgeFromDate_WhenBirthdayIsToday_ReturnsExactYearsElapsed()
        {
            var dateOfBirth = DateTime.Today.AddYears(-20);

            var age = dateOfBirth.GetAgeFromDate();

            Assert.Equal(20, age);
        }

        [Fact]
        public void GetAgeFromDate_WhenBirthdayAlreadyPassedThisYear_ReturnsFullYears()
        {
            var dateOfBirth = DateTime.Today.AddYears(-20).AddDays(-1);

            var age = dateOfBirth.GetAgeFromDate();

            Assert.Equal(20, age);
        }

        [Fact]
        public void GetAgeFromDate_WhenBirthdayNotYetReachedThisYear_ReturnsOneLess()
        {
            var dateOfBirth = DateTime.Today.AddYears(-20).AddDays(1);

            var age = dateOfBirth.GetAgeFromDate();

            Assert.Equal(19, age);
        }

        [Fact]
        public void GetTimeDifferenceString_WhenDateIsToday_ReturnsEmptyString()
        {
            var result = DateTime.Today.GetTimeDifferenceString();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetTimeDifferenceString_WithFortyDaysDifference_ReturnsMonthsAndDays()
        {
            var date = DateTime.Today.AddDays(-40);

            var result = date.GetTimeDifferenceString();

            Assert.Equal("1 months 9 days ", result);
        }

        [Fact]
        public void GetTimeDifferenceString_WhenDateIsInTheFuture_ThrowsArgumentOutOfRangeException()
        {
            var futureDate = DateTime.Today.AddDays(1);

            Assert.Throws<ArgumentOutOfRangeException>(() => futureDate.GetTimeDifferenceString());
        }
    }
}
