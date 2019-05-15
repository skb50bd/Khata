using System;
using Data.Persistence;
using Data.Persistence.DbViews;
using Microsoft.EntityFrameworkCore;

namespace SQLServerMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Migrating Database");

            var optionsBuilder = new DbContextOptionsBuilder<KhataContext>();
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=Khata;Trusted_Connection=True;MultipleActiveResultSets=true");

            var ctx = new KhataContext(optionsBuilder.Options);
            ctx.Database.Migrate();
            ctx.Database.CreateAllSQLServerViews();

            Console.WriteLine("Done");
        }
    }
}
