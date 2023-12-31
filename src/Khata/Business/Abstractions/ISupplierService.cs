﻿using System;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions
{
    public interface ISupplierService
    {
        Task<SupplierDto> Add(SupplierViewModel model);
        Task<SupplierDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SupplierDto> Get(int id);
        Task<IPagedList<SupplierDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SupplierDto> Remove(int id);
        Task<SupplierDto> Update(SupplierViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}