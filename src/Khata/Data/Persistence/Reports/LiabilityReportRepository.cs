using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Reports
{
    public class LiabilityReportRepository : IReportRepository<Liability>
    {
        protected readonly KhataContext Db;
        public LiabilityReportRepository(KhataContext db)
            => Db = db;

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
            var due = Db.Suppliers.Where(s => s.Payable > 0 && !s.IsRemoved)
                        .Select(s => s.Payable);
            var unpaidEmployees =
                Db.Employees.Where(e => e.Balance > 0 && !e.IsRemoved)
                  .Select(e => e.Balance);

            var c =
                await Db.CashRegister
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
