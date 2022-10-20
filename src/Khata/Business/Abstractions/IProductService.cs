using System;
using System.Threading.Tasks;
using Brotal;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface IProductService
{
    Task<IPagedList<ProductDto>> Get(int outletId, PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<ProductDto> Get(int id);
    Task<ProductDto> Add(ProductViewModel model);
    Task<ProductDto> Update(ProductViewModel vm);
    Task<ProductDto> Remove(int id);
    Task<bool> Exists(int id);
    Task<ProductDto> Delete(int id);
    Task<int> Count(DateTime? from, DateTime? to);
}