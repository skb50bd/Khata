using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.AspNetCore.Http;

namespace Khata.Services.CRUD
{
    public class CashRegisterService : ICashRegisterService
    {
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public CashRegisterService(IUnitOfWork db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CashRegister> Get()
            => await _db.CashRegister.Get();
        public async Task<CashRegister> GetWithTransactions()
            => await _db.CashRegister.GetWithTransactions();

        public async Task<CashRegister> Update(CashRegister model)
        {
            model.Metadata.Modified(CurrentUser);
            await _db.CompleteAsync();

            return model;
        }

        public async Task AddDeposit(IDeposit model)
        {
            var cr = await GetWithTransactions();
            cr.Deposits.Add(new Deposit(model));
            await _db.CompleteAsync();
        }

        public async Task AddWithdrawal(IWithdrawal model)
        {
            var cr = await GetWithTransactions();
            cr.Withdrawals.Add(new Withdrawal(model));
            await _db.CompleteAsync();
        }
    }
}
