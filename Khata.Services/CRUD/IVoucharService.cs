﻿using System;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface IVoucharService
    {
        Task<VoucharDto> Add(Vouchar model);
        Task<VoucharDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<VoucharDto> Get(int id);
        Task<IPagedList<VoucharDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<VoucharDto> Remove(int id);
        Task<VoucharDto> SetPurchase(int invoiceId, int saleId);
        Task<VoucharDto> SetSupplierPayment(int invoiceId, int debtPaymentId);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}