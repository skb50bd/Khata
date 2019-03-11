using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Data.Core;
using Domain;
using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Microsoft.AspNetCore.Http;

using Brotal.Extensions;

namespace Business.CRUD
{
    public class ExpenseService : IExpenseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public ExpenseService(IMapper mapper,
            IUnitOfWork db,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<ExpenseDto>> Get(PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Expense, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Name.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.Expenses.Get(
                predicate,
                p => p.Id,
                pf.PageIndex,
                pf.PageSize,
                from,
                to
            );
            return res.CastList(c => _mapper.Map<ExpenseDto>(c));
        }

        public async Task<ExpenseDto> Get(int id) =>
            _mapper.Map<ExpenseDto>(await _db.Expenses.GetById(id));

        public async Task<ExpenseDto> Add(ExpenseViewModel model)
        {
            var dm = _mapper.Map<Expense>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Expenses.Add(dm);
            var withdrawal = new Withdrawal(dm as IWithdrawal);
            withdrawal.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Withdrawals.Add(withdrawal);

            await _db.CompleteAsync();

            withdrawal.RowId = dm.RowId;
            await _db.CompleteAsync();

            return _mapper.Map<ExpenseDto>(dm);
        }

        public async Task<ExpenseDto> Update(ExpenseViewModel vm)
        {
            var newExpense = _mapper.Map<Expense>(vm);
            var originalExpense = await _db.Expenses.GetById(newExpense.Id);
            var meta = originalExpense.Metadata.Modified(CurrentUser);
            originalExpense.SetValuesFrom(newExpense);
            originalExpense.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<ExpenseDto>(originalExpense);
        }

        public async Task<ExpenseDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Expenses.IsRemoved(id))
                return null;
            await _db.Expenses.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<ExpenseDto>(await _db.Expenses.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Expenses.Exists(id);

        public async Task<ExpenseDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<ExpenseDto>(await _db.Expenses.GetById(id));
            await _db.Expenses.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<ExpenseDto>(dto);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Expenses.Count(from, to);
        }
    }
}
