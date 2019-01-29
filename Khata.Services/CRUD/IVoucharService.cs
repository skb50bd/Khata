using System.Threading.Tasks;
using Khata.Domain;
using Khata.Services.PageFilterSort;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IVoucharService
    {
        Task<Vouchar> Add(Vouchar model);
        Task<Vouchar> Delete(int id);
        Task<bool> Exists(int id);
        Task<Vouchar> Get(int id);
        Task<IPagedList<Vouchar>> Get(PageFilter pf);
        Task<Vouchar> Remove(int id);
        Task<Vouchar> SetPurchase(int invoiceId, int saleId);
        Task<Vouchar> SetSupplierPayment(int invoiceId, int debtPaymentId);
    }
}