using System;
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
    public class SalaryPaymentService : ISalaryPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SalaryPaymentService(IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<SalaryPaymentDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<SalaryPayment, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Employee.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.SalaryPayments.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<SalaryPaymentDto>(c));
        }

        public async Task<SalaryPaymentDto> Get(int id)
        {
            return _mapper.Map<SalaryPaymentDto>(await _db.SalaryPayments.GetById(id));
        }

        public async Task<SalaryPaymentDto> Add(SalaryPaymentViewModel model)
        {
            var emp = await _db.Employees.GetById(model.EmployeeId);
            var dm = _mapper.Map<SalaryPayment>(model);

            dm.BalanceBefore = emp.Balance;
            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            emp.Balance -= model.Amount;
            _db.SalaryPayments.Add(dm);

            var withdrawal = new Withdrawal(dm as IWithdrawal);
            withdrawal.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Withdrawals.Add(withdrawal);
            await _db.CompleteAsync();

            withdrawal.RowId = dm.RowId;
            await _db.CompleteAsync();

            return _mapper.Map<SalaryPaymentDto>(dm);
        }

        public async Task<SalaryPaymentDto> Update(SalaryPaymentViewModel vm)
        {
            var newSalaryPayment = _mapper.Map<SalaryPayment>(vm);
            var originalSalaryPayment = await _db.SalaryPayments.GetById(newSalaryPayment.Id);
            var meta = originalSalaryPayment.Metadata.Modified(CurrentUser);
            originalSalaryPayment.SetValuesFrom(newSalaryPayment);
            originalSalaryPayment.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<SalaryPaymentDto>(originalSalaryPayment);
        }

        public async Task<SalaryPaymentDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.SalaryPayments.IsRemoved(id))
                return null;
            await _db.SalaryPayments.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<SalaryPaymentDto>(await _db.SalaryPayments.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.SalaryPayments.Exists(id);

        public async Task<SalaryPaymentDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<SalaryPaymentDto>(await _db.SalaryPayments.GetById(id));
            await _db.SalaryPayments.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<SalaryPaymentDto>(dto);
        }
    }
}
