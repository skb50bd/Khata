using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.PageFilterSort;
using Domain;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface IPurchaseReturnService
{
    Task<PurchaseReturnDto> Add(PurchaseReturnViewModel model);
    Task<int> Count(DateTime? from = null, DateTime? to = null);
    Task<PurchaseReturnDto> Delete(int id);
    Task<bool> Exists(int id);
    Task<PurchaseReturnDto> Get(int id);
    Task<IEnumerable<PurchaseReturnDto>> GetSupplierPurchaseReturns(int supplierId);
    Task<IPagedList<PurchaseReturnDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<PurchaseReturnDto> Remove(int id);
    Task<PurchaseReturnDto> Update(PurchaseReturnViewModel vm);
}