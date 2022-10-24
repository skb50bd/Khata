namespace Domain.Utils;

public static class DateTimeExtensions
{
    public static DateOnly StartOfTheWeek(
            this DateOnly date,
            DayOfWeek startOfWeek) =>
        date.AddDays(-1 * ((7 + (date.DayOfWeek - startOfWeek)) % 7));

    public static DateOnly StartOfTheMonth(this DateOnly date) =>
        new(date.Year, date.Month, 0);
}