using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.PageFilterSort;
using Domain;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface ISaleService
{
    Task<SaleDto> Add(SaleViewModel model);
    Task<SaleDto> Delete(int id);
    Task<bool> Exists(int id);
    Task<SaleDto> Get(int id);
    Task<IEnumerable<SaleDto>> GetCustomerSales(int customerId, DateTime? from = null, DateTime? to = null);
    Task<IPagedList<SaleDto>> Get(int outletId, PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<SaleDto> Remove(int id);
    Task<SaleDto> Update(SaleViewModel vm);
    Task<int> Count(DateTime? from, DateTime? to);

    Task<SaleDto> Save(SaleViewModel model);
    Task<SaleDto> GetSaved(int id);
    Task<IEnumerable<SaleDto>> GetSaved();
    Task<SaleDto> DeleteSaved(int id);
    Task DeleteAllSaved();
}