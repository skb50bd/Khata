using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Reports;

public class CustomerReportRepository 
    : IIndividualReportRepository<CustomerReport>
{
    private readonly KhataContext _db;
    public CustomerReportRepository(
        KhataContext db) =>
        _db = db;

    public async Task<CustomerReport> GetById(int id) =>
        (await _db.Customers.Include(c => c.Purchases)
            .ThenInclude(s => s.Cart)
            .Include(c => c.DebtPayments)
            .Include(c => c.Refunds)
            .Include(c => c.Metadata)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync()
        ).GetReport();
}