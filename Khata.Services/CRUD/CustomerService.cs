using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Http;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public CustomerService(IUnitOfWork db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<CustomerDto>> Get(
            PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Customer, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.FullName.ToLowerInvariant().Contains(pf.Filter)
                    || p.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Phone.Contains(pf.Filter)
                    || p.Email.Contains(pf.Filter);

            var res = await _db.Customers.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize, from, to);
            return res.CastList(c => _mapper.Map<CustomerDto>(c));
        }

        public async Task<CustomerDto> Get(int id)
        {
            return _mapper.Map<CustomerDto>(await _db.Customers.GetById(id));
        }

        public async Task<CustomerDto> Add(CustomerViewModel model)
        {
            var dm = _mapper.Map<Customer>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Customers.Add(dm);
            await _db.CompleteAsync();

            return _mapper.Map<CustomerDto>(dm);
        }

        public async Task<CustomerDto> Update(CustomerViewModel vm)
        {
            var newCustomer = _mapper.Map<Customer>(vm);
            var originalCustomer = await _db.Customers.GetById(newCustomer.Id);
            var meta = originalCustomer.Metadata.Modified(CurrentUser);
            originalCustomer.SetValuesFrom(newCustomer);
            originalCustomer.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<CustomerDto>(originalCustomer);
        }

        public async Task<CustomerDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Customers.IsRemoved(id))
                return null;
            await _db.Customers.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<CustomerDto>(await _db.Customers.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Customers.Exists(id);

        public async Task<CustomerDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<CustomerDto>(await _db.Customers.GetById(id));
            await _db.Customers.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<CustomerDto>(dto);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Customers.Count(from, to);
        }
    }
}
