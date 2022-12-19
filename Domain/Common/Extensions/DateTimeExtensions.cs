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

        public static string GetTimeDifferenceString(this DateTime date)
        {
            var today = DateTime.Today;
            var timeSpanDifference = today - date;
            var dateDifference = DateTime.MinValue + timeSpanDifference;
            int Year = dateDifference.Year - 1;
            int Months = dateDifference.Month - 1;
            int Days = dateDifference.Day - 1;
            var result = string.Concat(
                Year != 0 ? Year + " years " : null, 
                Months != 0 ? Months + " months " : null, 
                Days != 0 ? Days + " days " : null);
            return result;
        }
    }
}