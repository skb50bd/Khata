﻿using System;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ISalaryPaymentService
    {
        Task<SalaryPaymentDto> Add(SalaryPaymentViewModel model);
        Task<SalaryPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SalaryPaymentDto> Get(int id);
        Task<IPagedList<SalaryPaymentDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SalaryPaymentDto> Remove(int id);
        Task<SalaryPaymentDto> Update(SalaryPaymentViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}