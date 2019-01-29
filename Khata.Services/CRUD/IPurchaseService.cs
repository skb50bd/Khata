﻿using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IPurchaseService
    {
        Task<PurchaseDto> Add(PurchaseViewModel model);
        Task<PurchaseDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<PurchaseDto> Get(int id);
        Task<IPagedList<PurchaseDto>> Get(PageFilter pf);
        Task<PurchaseDto> Remove(int id);
        Task<PurchaseDto> Update(PurchaseViewModel vm);
    }
}