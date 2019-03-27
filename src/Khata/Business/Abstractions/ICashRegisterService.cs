using System.Threading.Tasks;

using Domain;

namespace Business.Abstractions
{
    public interface ICashRegisterService
    {
        Task<CashRegister> Get();
    }
}