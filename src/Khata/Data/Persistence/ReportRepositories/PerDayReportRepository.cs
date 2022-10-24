using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using static System.Decimal;

namespace Data.Persistence.Reports;

public class PerDayReportRepository : IListReportRepository<PerDayReport>
{
    private readonly IDateTimeProvider _dateTime;
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public PerDayReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings, 
        IDateTimeProvider dateTime)
    {
        _db       = db;
        _dateTime = dateTime;
        _settings = settings.CurrentValue;
    }

    public async Task<IEnumerable<PerDayReport>> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            return await _db.Set<PerDayReport>().ToListAsync();
        }

        const int days = 30;

        var deposits =
            _db.Set<Deposit>()
                .Include(d => d.Metadata)
                .Where(d => d.Metadata.CreationTime >= _dateTime.TodayAsDateTime.AddDays(-days + 1))
                .Select(d => new PerDayReport
                {
                    Date = d.Metadata.CreationTime.Date,
                    CashIn = d.Amount
                });

        var withdrawals =
            _db.Set<Withdrawal>()
                .Include(w => w.Metadata)
                .Where(d => d.Metadata.CreationTime >= _dateTime.TodayAsDateTime.AddDays(-days + 1))
                .Select(w => new PerDayReport
                {
                    Date = w.Metadata.CreationTime.Date,
                    CashOut = w.Amount
                });

        var sales =
            _db.Set<Sale>()
                .Include(s => s.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >= _dateTime.TodayAsDateTime.AddDays(-days + 1) 
                    && d.IsRemoved == false)
                .Select(s => new PerDayReport
                {
                    Date = s.Metadata.CreationTime.Date,
                    NewReceivable = s.Payment.Due
                });

        var purchases =
            _db.Set<Purchase>()
                .Include(p => p.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >= _dateTime.TodayAsDateTime.AddDays(-days + 1) 
                    && d.IsRemoved == false)
                .Select(p => new PerDayReport
                {
                    Date = p.Metadata.CreationTime
                            .Date,
                    NewPayable = p.Payment.Due
                });

        var salaryIssues =
            _db.Set<SalaryIssue>()
                .Include(si => si.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >= _dateTime.TodayAsDateTime.AddDays(-days + 1) 
                    && d.IsRemoved == false)
                .Select(si => new PerDayReport
                {
                    Date = si.Metadata.CreationTime.Date,
                    NewPayable = si.Amount
                });

        return await deposits
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
            .OrderByDescending(i => i.Date)
            .ToListAsync();
    }
}