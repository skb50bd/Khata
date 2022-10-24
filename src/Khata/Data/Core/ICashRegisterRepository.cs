using Domain;

namespace Data.Core;

public interface ICashRegisterRepository
{
    Task<CashRegister?> Get();
}