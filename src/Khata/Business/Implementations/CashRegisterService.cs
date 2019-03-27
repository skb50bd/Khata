using System.Threading.Tasks;

using Business.Abstractions;

using Data.Core;

using Domain;

using Microsoft.AspNetCore.Http;

namespace Business.Implementations
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

        public async Task<CashRegister> Update(CashRegister model)
        {
            model.Metadata.Modified(CurrentUser);
            await _db.CompleteAsync();

            return model;
        }
    }
}
