using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.PageFilterSort;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IInvoiceService
    {
        Task<Invoice> Add(Invoice model);
        Task<Invoice> SetSale(int invoiceId, int saleId);
        Task<Invoice> SetDebtPayment(int invoiceId, int debtPaymentId);
        Task<Invoice> Delete(int id);
        Task<bool> Exists(int id);
        Task<Invoice> Get(int id);
        Task<IPagedList<Invoice>> Get(PageFilter pf);
        Task<Invoice> Remove(int id);
    }
}