﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Brotal;
using Brotal.Extensions;
using Business.Abstractions;
using Business.PageFilterSort;
using Data.Core;
using Domain;
using Microsoft.AspNetCore.Http;
using ViewModels;
using Metadata = Domain.Metadata;

namespace Business.Implementations
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public TransactionsService(IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<Deposit>> GetDeposits(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Deposit, bool>>)(d => true)
                : d => d.Id.ToString() == pf.Filter
                    || d.Description == pf.Filter;

            var res = await _db.Deposits.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res;
        }

        public async Task<IPagedList<Withdrawal>> GetWithdrawals(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Withdrawal, bool>>)(d => true)
                : d => d.Id.ToString() == pf.Filter
                    || d.Description == pf.Filter;

            var res = await _db.Withdrawals.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res;
        }

        public async Task<IEnumerable<Deposit>> GetDeposits(DateTime from, DateTime to)
        {
            return await _db.Deposits.Get(d => true, d => d.Id, 1, int.MaxValue, from, to);
        }

        public async Task<IEnumerable<Withdrawal>> GetWithdrawals(DateTime from, DateTime to)
        {
            return await _db.Withdrawals.Get(d => true, d => d.Id, 1, int.MaxValue, from, to);
        }

        public async Task<Deposit> GetDepositById(int id)
            => await _db.Deposits.GetById(id);

        public async Task<Withdrawal> GetWithdrawalById(int id)
            => await _db.Withdrawals.GetById(id);

        public async Task<Deposit> Add(DepositViewModel model)
        {
            var dm = _mapper.Map<Deposit>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Deposits.Add(dm);
            await _db.CompleteAsync();
            return dm;
        }

        public async Task<Withdrawal> Add(WithdrawalViewModel model)
        {
            var dm = _mapper.Map<Withdrawal>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Withdrawals.Add(dm);
            await _db.CompleteAsync();
            return dm;
        }

        public async Task<Deposit> DeleteDeposit(int id)
        {
            if (!(await DepositExists(id)))
                return null;
            await _db.Deposits.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<Deposit>(await _db.Deposits.GetById(id));
        }

        public async Task<Withdrawal> DeleteWithdrawal(int id)
        {
            if (!(await WithdrawalExists(id)))
                return null;
            await _db.Withdrawals.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<Withdrawal>(await _db.Withdrawals.GetById(id));
        }

        public async Task<bool> DepositExists(int id) => await _db.Deposits.Exists(id);
        public async Task<bool> WithdrawalExists(int id) => await _db.Withdrawals.Exists(id);
    }
}
