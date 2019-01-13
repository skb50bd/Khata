using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface IUnitOfWork
    {
        ITrackingRepository<Product> Products { get; }
        ITrackingRepository<Service> Services { get; }
        ITrackingRepository<Customer> Customers { get; }
        ITrackingRepository<Sale> Sales { get; }
        ITrackingRepository<DebtPayment> DebtPayments { get; }
        ITrackingRepository<Expense> Expenses { get; }
        ITrackingRepository<Supplier> Suppliers { get; }
        ITrackingRepository<SupplierPayment> SupplierPayments { get; }
        ITrackingRepository<Purchase> Purchases { get; }
        ITrackingRepository<Employee> Employees { get; }
        ITrackingRepository<SalaryIssue> SalaryIssues { get; }
        ITrackingRepository<SalaryPayment> SalaryPayments { get; }
        void Complete();

        Task CompleteAsync();
    }
}