using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Brotal.Extensions;

namespace Business.CRUD
{
    public interface ISupplierPaymentService
    {
        Task<SupplierPaymentDto> Add(SupplierPaymentViewModel model);
        Task<SupplierPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SupplierPaymentDto> Get(int id);
        Task<IEnumerable<SupplierPaymentDto>> GetSupplierPayments(int supplierId);
        Task<IPagedList<SupplierPaymentDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SupplierPaymentDto> Remove(int id);
        Task<SupplierPaymentDto> Update(SupplierPaymentViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}