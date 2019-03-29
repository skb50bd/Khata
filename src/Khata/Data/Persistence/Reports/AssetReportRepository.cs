using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

using static System.Decimal;

namespace Data.Persistence.Reports
{
    public class AssetReportRepository: IReportRepository<Asset>
    {
        protected readonly KhataContext Db;
        public AssetReportRepository(KhataContext db)
            => Db = db;

        public async Task<Asset> Get() =>
            new Asset
            {
                DueCount =
                    await Db.Customers.CountAsync(c => !c.IsRemoved && c.Debt > 0),
                TotalDue = 
                    Round(await Db.Customers.Where(c => !c.IsRemoved)
                        .SumAsync(c => c.Debt),
                    2),
                Cash = 
                    Round(
                        (await Db.CashRegister.FirstOrDefaultAsync())?.Balance ?? 0M,
                    2),
                InventoryCount = 
                    await Db.Products.CountAsync(p =>
                        !p.IsRemoved &&
                        p.Inventory.TotalStock > 0),
                InventoryWorth = 
                Round(await Db.Products.Where(p => !p.IsRemoved)
                    .SumAsync(p =>
                        p.Inventory
                        .TotalStock *
                        p.Price.Purchase), 
                2)
            };
    }
}
