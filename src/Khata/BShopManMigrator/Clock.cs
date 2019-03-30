using System;

namespace BShopManMigrator
{
    public static class Clock
    {
        public static DateTime Now => Clock.Now;
        public static DateTime Today => Clock.Today;
        public static DateTime Max => new DateTime(3000, 12, 31);
        public static DateTime Min => new DateTime(1990, 1, 1);
    }
}
