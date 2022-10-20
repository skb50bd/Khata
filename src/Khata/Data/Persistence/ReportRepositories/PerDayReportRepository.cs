using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using static System.Decimal;

namespace Data.Persistence.Reports;

public class PerDayReportRepository : IListReportRepository<PerDayReport>
{
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public PerDayReportRepository(
        KhataContext                   db,
        IOptionsMonitor<KhataSettings> settings)
    {
        _db       = db;
        _settings = settings.CurrentValue;
    }

    public async Task<IEnumerable<PerDayReport>> Get()

    {
        if (_settings.DbProvider == DbProvider.SQLServer)
            return await _db.Set<PerDayReport>()
                .ToListAsync();

        const int days = 30;
        var deposits =
            await _db.Deposits.Include(d => d.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >=
                    Clock.Today.AddDays(-days + 1))
                .Select(
                    d => new PerDayReport
                    {
                        Date = d
                            .Metadata.CreationTime
                            .Date,
                        CashIn = d.Amount
                    }).ToListAsync();

        var withdrawals =
            await _db.Withdrawals.Include(w => w.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >=
                    Clock.Today.AddDays(
                        -days + 1))
                .Select(
                    w => new PerDayReport
                    {
                        Date = w
                            .Metadata
                            .CreationTime
                            .Date,
                        CashOut = w.Amount
                    }).ToListAsync();

        var sales =
            await _db.Sales.Include(s => s.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >=
                    Clock.Today.AddDays(-days + 1) &&
                    !d.IsRemoved)
                .Select(
                    s => new PerDayReport
                    {
                        Date =
                            s.Metadata.CreationTime.Date,
                        NewReceivable = s.Payment.Due
                    }).ToListAsync();

        var purchases =
            await _db.Purchases.Include(p => p.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >=
                    Clock.Today.AddDays(
                        -days + 1) &&
                    !d.IsRemoved)
                .Select(
                    p => new PerDayReport
                    {
                        Date =
                            p.Metadata.CreationTime
                                .Date,
                        NewPayable = p.Payment.Due
                    }).ToListAsync();

        var salaryIssues =
            await _db.SalaryIssues.Include(si => si.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >=
                    Clock.Today.AddDays(
                        -days + 1) &&
                    !d.IsRemoved)
                .Select(
                    si => new PerDayReport
                    {
                        Date = si
                            .Metadata
                            .CreationTime
                            .Date,
                        NewPayable = si.Amount
                    }).ToListAsync();

        return deposits
            .Union(withdrawals)
            .Union(sales)
            .Union(purchases)
            .Union(salaryIssues)
            .GroupBy(
                i => i.Date,
                i => i,
                (key, g) => new PerDayReport
                {
                    Date = key,
                    CashIn = Round(
                        g.Sum(r => r.CashIn), 2),
                    CashOut = Round(
                        g.Sum(r => r.CashOut), 2),
                    NewReceivable = Round(
                        g.Sum(r => r.NewReceivable), 2),
                    NewPayable = Round(
                        g.Sum(r => r.NewPayable), 2)
                })
            .OrderByDescending(i => i.Date);
    }
}