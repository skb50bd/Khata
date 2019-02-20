using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface IRefundService
    {
        Task<RefundDto> Add(RefundViewModel model);
        Task<RefundDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<RefundDto> Get(int id);
        Task<IEnumerable<RefundDto>> GetCustomerRefunds(int customerId);
        Task<IPagedList<RefundDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<RefundDto> Remove(int id);
        Task<RefundDto> Update(RefundViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}