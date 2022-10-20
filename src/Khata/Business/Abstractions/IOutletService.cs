using System.Collections.Generic;
using System.Threading.Tasks;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface IOutletService
{
    Task<IEnumerable<OutletDto>> Get();
    Task<OutletDto> Get(int id);
    Task<OutletDto> Add(OutletViewModel model);
    Task<OutletDto> Update(OutletViewModel vm);
    Task<OutletDto> Remove(int id);
    Task<bool> Exists(int id);
    Task<OutletDto> Delete(int id);
    Task<int> Count();
}