﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Http;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class SupplierPaymentService : ISupplierPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SupplierPaymentService(IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<SupplierPaymentDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<SupplierPayment, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Supplier.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.SupplierPayments.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<SupplierPaymentDto>(c));
        }

        public async Task<SupplierPaymentDto> Get(int id)
        {
            return _mapper.Map<SupplierPaymentDto>(await _db.SupplierPayments.GetById(id));
        }

        public async Task<SupplierPaymentDto> Add(SupplierPaymentViewModel model)
        {
            var dm = _mapper.Map<SupplierPayment>(model);
            dm.Supplier = await _db.Suppliers.GetById(model.SupplierId);
            dm.PayableBefore = dm.Supplier.Payable;
            dm.Supplier.Payable -= model.Amount;

            dm.Vouchar = _mapper.Map<Vouchar>(dm);
            dm.Vouchar.Metadata = Metadata.CreatedNew(CurrentUser);
            dm.Vouchar.SupplierPayment = dm;

            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            var withdrawal = new Withdrawal(dm as IWithdrawal)
            {
                Metadata = Metadata.CreatedNew(CurrentUser)
            };

            _db.SupplierPayments.Add(dm);
            _db.Withdrawals.Add(withdrawal);
            await _db.CompleteAsync();

            withdrawal.RowId = dm.RowId;
            await _db.CompleteAsync();

            return _mapper.Map<SupplierPaymentDto>(dm);
        }

        public async Task<SupplierPaymentDto> Update(SupplierPaymentViewModel vm)
        {
            var newSupplierPayment = _mapper.Map<SupplierPayment>(vm);
            var originalSupplierPayment = await _db.SupplierPayments.GetById(newSupplierPayment.Id);
            var meta = originalSupplierPayment.Metadata.Modified(CurrentUser);
            originalSupplierPayment.SetValuesFrom(newSupplierPayment);
            originalSupplierPayment.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<SupplierPaymentDto>(originalSupplierPayment);
        }

        public async Task<SupplierPaymentDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.SupplierPayments.IsRemoved(id))
                return null;
            await _db.SupplierPayments.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<SupplierPaymentDto>(await _db.SupplierPayments.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.SupplierPayments.Exists(id);

        public async Task<SupplierPaymentDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<SupplierPaymentDto>(await _db.SupplierPayments.GetById(id));
            await _db.SupplierPayments.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<SupplierPaymentDto>(dto);
        }
    }
}
