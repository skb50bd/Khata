﻿using System;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface IExpenseService
    {
        Task<ExpenseDto> Add(ExpenseViewModel model);
        Task<ExpenseDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<ExpenseDto> Get(int id);
        Task<IPagedList<ExpenseDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<ExpenseDto> Remove(int id);
        Task<ExpenseDto> Update(ExpenseViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}