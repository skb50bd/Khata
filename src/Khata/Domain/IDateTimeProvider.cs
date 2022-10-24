namespace Domain;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
    DateOnly Today { get; }
    DateTime TodayAsDateTime { get; }
    TimeOnly TimeOfDay { get; }
    DateTimeOffset ParseDateTime(string str);
    DateOnly ParseDate(string dateStr);
    TimeOnly ParseTime(string timeStr);
    DateTimeOffset Min { get; }
    DateTimeOffset Max { get; }
}