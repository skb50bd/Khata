using System;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ICustomerInvoiceService
    {
        Task<CustomerInvoiceDto> Add(CustomerInvoice model);
        Task<CustomerInvoiceDto> SetSale(int invoiceId, int saleId);
        Task<CustomerInvoiceDto> SetDebtPayment(int invoiceId, int debtPaymentId);
        Task<CustomerInvoiceDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<CustomerInvoiceDto> Get(int id);
        Task<IPagedList<CustomerInvoiceDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<CustomerInvoiceDto> Remove(int id);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}