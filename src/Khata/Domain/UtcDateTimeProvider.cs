namespace Domain;

public class UtcDateTimeProvider : LocalDateTimeProvider
{
    protected override DateTimeOffset GetCurrentDateTime() => DateTimeOffset.UtcNow;
}