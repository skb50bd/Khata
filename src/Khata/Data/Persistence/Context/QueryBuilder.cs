
using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence;

public static class QueryBuilder
{
       public static ModelBuilder BuildQueries(
              this ModelBuilder builder,
              KhataContext db)
       {
              builder.Entity<Asset>()
                     .HasNoKey()
                     .ToView("Asset");

              builder.Entity<Liability>()
                     .HasNoKey()
                     .ToView("Liability");

              builder.Entity<PerDayReport>()
                     .HasNoKey()
                     .ToView("PerDayReport");

              builder.Entity<Inflow>()
                     .HasNoKey()
                     .ToView("PeriodicalInflow");

              builder.Entity<Outflow>()
                     .HasNoKey()
                     .ToView("PeriodicalOutflow");

              builder.Entity<Payable>()
                     .HasNoKey()
                     .ToView("PeriodicalPayableReport");

              builder.Entity<Receivable>()
                     .HasNoKey()
                     .ToView("PeriodicalReceivableReport");

              return builder;
       }
}