using System.Threading.Tasks;
using Khata.Domain;

namespace Khata.Services.CRUD
{
    public interface ICashRegisterService
    {
        Task AddDeposit(IDeposit model);
        Task AddWithdrawal(IWithdrawal model);
        Task<CashRegister> Get();
        Task<CashRegister> GetWithTransactions();
        Task<CashRegister> Update(CashRegister model);
    }
}