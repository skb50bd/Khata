using System;

namespace Domain;

public static class Clock
{
    public static DateTime Now => DateTime.Now;
    public static DateTime Today => DateTime.Today;
    public static DateTime Max => new DateTime(3000, 12, 31);
    public static DateTime Min => new DateTime(2000, 01, 01);
}