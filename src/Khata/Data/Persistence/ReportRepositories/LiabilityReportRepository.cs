using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Persistence.Reports;

public class LiabilityReportRepository : IReportRepository<Liability>
{
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public LiabilityReportRepository(
        KhataContext                   db,
        IOptionsMonitor<KhataSettings> settings)
    {
        _db       = db;
        _settings = settings.CurrentValue;
    }

    //public async Task<Liability> Get() =>
    //    new Liability
    //    {
    //        DueCount =
    //            await Db.Suppliers.CountAsync(
    //                c => !c.IsRemoved && c.Payable > 0),
    //        TotalDue =
    //            Round(await Db.Suppliers
    //                       .Where(c => !c.IsRemoved)
    //                       .SumAsync(c => c.Payable), 2),
    //        UnpaidEmployees =
    //            await Db.Employees.CountAsync(
    //                p => !p.IsRemoved && p.Balance > 0),
    //        UnpaidAmount =
    //            Round(await Db.Employees
    //                       .Where(p => !p.IsRemoved)
    //                       .SumAsync(p => p.Balance), 2)
    //    };

    public async Task<Liability?> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            return await _db.Set<Liability>().FirstOrDefaultAsync();
        }

        var dueQuery = 
            _db.Set<Supplier>()
                .Where(s => 
                    s.Payable > 0 
                    && s.IsRemoved == false
                )
                .Select(s => s.Payable);

        var dueCountTask = dueQuery.CountAsync();
        var dueTotalTask = dueQuery.SumAsync();
        
        var unpaidEmployeesQuery =
            _db.Set<Employee>()
                .Where(e => 
                    e.Balance > 0 
                    && e.IsRemoved == false
                )
                .Select(e => e.Balance);

        var unpaidEmployeesCountTask = unpaidEmployeesQuery.CountAsync();
        var unpaidSalarySumTask = unpaidEmployeesQuery.SumAsync();

        var cashTask = 
            _db.Set<CashRegister>().FirstOrDefaultAsync();
        
        await Task.WhenAll(
            dueCountTask,
            dueTotalTask,
            unpaidEmployeesCountTask,
            unpaidSalarySumTask,
            cashTask
        );
        
        return new Liability
        {
            TotalDue        = await dueTotalTask,
            DueCount        = await dueCountTask,
            UnpaidAmount    = await unpaidSalarySumTask,
            UnpaidEmployees = await unpaidEmployeesCountTask
        };
    }
}