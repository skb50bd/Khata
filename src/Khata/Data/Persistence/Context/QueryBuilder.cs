using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public static class QueryBuilder
    {
        public static ModelBuilder BuildQueries(
            this ModelBuilder builder)
        {
            builder.Query<Asset>().ToView("Asset");
            builder.Query<Liability>().ToView("Liability");
            builder.Query<PerDayReport>().ToView("PerDayReport");
            builder.Query<Inflow>().ToView("PeriodicalInflow");
            builder.Query<Outflow>().ToView("PeriodicalOutflow");
            builder.Query<Payable>().ToView("PeriodicalPayableReport");
            builder.Query<Receivable>().ToView("PeriodicalReceivableReport");

            return builder;
        }
    }
}
