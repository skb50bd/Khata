using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Services.CRUD
{
    public interface ICashRegisterService
    {
        Task<CashRegister> Get();
    }
}