namespace ZFlex.Extensions
{
    public static class DateExtensions
    {
        public static bool IsWorkingDay(this DateOnly date)
            => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }
}
