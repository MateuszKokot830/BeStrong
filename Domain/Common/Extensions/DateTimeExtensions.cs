namespace Domain.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetAgeFromDate(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}