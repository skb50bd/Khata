using System.Threading.Tasks;

using Domain;

namespace Business.CRUD
{
    public interface ICashRegisterService
    {
        Task<CashRegister> Get();
    }
}