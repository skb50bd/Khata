using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.PageFilterSort;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IInvoiceService
    {
        Task<CustomerInvoice> Add(CustomerInvoice model);
        Task<CustomerInvoice> SetSale(int invoiceId, int saleId);
        Task<CustomerInvoice> SetDebtPayment(int invoiceId, int debtPaymentId);
        Task<CustomerInvoice> Delete(int id);
        Task<bool> Exists(int id);
        Task<CustomerInvoice> Get(int id);
        Task<IPagedList<CustomerInvoice>> Get(PageFilter pf);
        Task<CustomerInvoice> Remove(int id);
    }
}