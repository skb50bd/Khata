using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface ICashRegisterRepository
    {
        Task<CashRegister> Get();
    }
}
