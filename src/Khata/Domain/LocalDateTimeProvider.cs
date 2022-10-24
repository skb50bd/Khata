namespace Domain;

public class LocalDateTimeProvider : IDateTimeProvider
{
    protected virtual DateTimeOffset GetCurrentDateTime() => DateTimeOffset.Now;

    public virtual DateTimeOffset Now => GetCurrentDateTime();
    public virtual DateOnly Today => DateOnly.FromDateTime(Now.DateTime);
    public virtual DateTime TodayAsDateTime => Today.ToDateTime(TimeOnly.MinValue);
    public virtual TimeOnly TimeOfDay => TimeOnly.FromDateTime(Now.DateTime);
    public virtual DateTimeOffset ParseDateTime(string str) => DateTimeOffset.Parse(str);
    public virtual DateOnly ParseDate(string dateStr) => DateOnly.Parse(dateStr);
    public virtual TimeOnly ParseTime(string timeStr) => TimeOnly.Parse(timeStr);
    public virtual DateTimeOffset Min => DateTimeOffset.MinValue;
    public virtual DateTimeOffset Max => DateTimeOffset.MaxValue;
}