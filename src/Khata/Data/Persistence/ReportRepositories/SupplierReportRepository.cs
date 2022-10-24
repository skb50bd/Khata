using Data.Core;
using Domain;
using Domain.Reports;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Reports;

public class SupplierReportRepository
    : IIndividualReportRepository<SupplierReport>
{
    private readonly KhataContext _db;
    public SupplierReportRepository(KhataContext db) => _db = db;

    public async Task<SupplierReport?> GetById(int id) =>
        (await _db.Set<Supplier>()
            .Include(s => s.Purchases)
            .Include(s => s.Payments)
            .Include(s => s.PurchaseReturns)
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync()
        )?.GetReport();
}