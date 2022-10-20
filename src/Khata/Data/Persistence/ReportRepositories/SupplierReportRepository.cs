using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Reports;

public class SupplierReportRepository
    : IIndividualReportRepository<SupplierReport>
{
    private readonly KhataContext _db;
    public SupplierReportRepository(
        KhataContext db) =>
        _db = db;

    public async Task<SupplierReport> GetById(int id) =>
        (await _db.Suppliers.Include(s => s.Purchases)
            .Include(s => s.Payments)
            .Include(s => s.PurchaseReturns)
            .Include(s => s.Metadata)
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync()
        ).GetReport();
}