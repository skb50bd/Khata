using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Throw;

namespace Data.Persistence.Reports;

public class AssetReportRepository : IReportRepository<Asset>
{
    private readonly KhataContext _db;
    private readonly KhataSettings _settings;

    public AssetReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings)
    {
        _db = db;
        _settings = settings.CurrentValue;
    }

    //public async Task<Asset> Get() =>
    //    new Asset
    //    {
    //        DueCount =
    //            await Db.Customers.CountAsync(c => !c.IsRemoved && c.Debt > 0),
    //        TotalDue = 
    //            Round(await Db.Customers.Where(c => !c.IsRemoved)
    //                .SumAsync(c => c.Debt),
    //            2),
    //        Cash = 
    //            Round(
    //                (await Db.CashRegister.FirstOrDefaultAsync())?.Balance ?? 0M,
    //            2),
    //        InventoryCount = 
    //            await Db.Products.CountAsync(p =>
    //                !p.IsRemoved &&
    //                p.Inventory.TotalStock > 0),
    //        InventoryWorth = 
    //        Round(await Db.Products.Where(p => !p.IsRemoved)
    //            .SumAsync(p =>
    //                p.Inventory
    //                .TotalStock *
    //                p.Price.Purchase), 
    //        2)
    //    };


    public async Task<Asset?> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            return await _db.Set<Asset>().FirstOrDefaultAsync();
        }

        var debts = 
            _db.Set<Customer>()
                .Where(c => c.Debt > 0 && !c.IsRemoved)
                .Select(c => c.Debt);

        var debtsSumTask = debts.SumAsync();
        var debtsCountTask = debts.CountAsync();
        
        var inventory =
            _db.Set<Product>()
                .Where(p => 
                    p.Inventory.Stock + p.Inventory.Warehouse > 0
                    && p.IsRemoved == false)
                .Select(p => 
                    p.Price.Purchase 
                    * (
                        p.Inventory.Stock 
                        + p.Inventory.Warehouse
                    )
                );

        var inventorySumTask = inventory.SumAsync();
        var inventoryCountTask = inventory.CountAsync();

        var cashTask = _db.Set<CashRegister>().FirstOrDefaultAsync();
        
        await Task.WhenAll(
            debtsSumTask, 
            debtsCountTask, 
            inventorySumTask, 
            inventoryCountTask,
            cashTask
        );

        var cash = await cashTask;
        cash.ThrowIfNull();

        return new Asset
        {
            Cash           = cash.Balance,
            DueCount       = await debtsCountTask,
            TotalDue       = await debtsSumTask,
            InventoryCount = await inventoryCountTask,
            InventoryWorth = await inventorySumTask
        };
    }
}