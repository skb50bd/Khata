using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Persistence.Reports
{
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

        public async Task<Liability> Get()
        {
            if (_settings.DbProvider == DbProvider.SQLServer)
                return await _db.Query<Liability>()
                                .FirstOrDefaultAsync();

            var due = _db.Suppliers.Where(s => s.Payable > 0 && !s.IsRemoved)
                        .Select(s => s.Payable);
            var unpaidEmployees =
                _db.Employees.Where(e => e.Balance > 0 && !e.IsRemoved)
                  .Select(e => e.Balance);

            var c =
                await _db.CashRegister
                    .Select(
                        cr => new Liability
                        {
                            TotalDue        = due.Sum(),
                            DueCount        = due.Count(),
                            UnpaidAmount    = unpaidEmployees.Sum(),
                            UnpaidEmployees = unpaidEmployees.Count()
                        }).FirstOrDefaultAsync();
            return c;
        }
    }
}
