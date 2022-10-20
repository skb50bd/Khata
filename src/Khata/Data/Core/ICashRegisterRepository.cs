using System.Threading.Tasks;

using Domain;

namespace Data.Core;

public interface ICashRegisterRepository
{
    Task<CashRegister> Get();
}