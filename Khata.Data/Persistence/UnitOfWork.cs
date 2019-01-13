using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

namespace Khata.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly KhataContext Context;

        public ITrackingRepository<Product> Products { get; }
        public ITrackingRepository<Service> Services { get; }
        public ITrackingRepository<Customer> Customers { get; }
        public ITrackingRepository<DebtPayment> DebtPayments { get; }
        public ITrackingRepository<Sale> Sales { get; }
        public ITrackingRepository<Expense> Expenses { get; }
        public ITrackingRepository<Supplier> Suppliers { get; }
        public ITrackingRepository<SupplierPayment> SupplierPayments { get; }
        public ITrackingRepository<Purchase> Purchases { get; }
        public ITrackingRepository<Employee> Employees { get; }
        public ITrackingRepository<SalaryIssue> SalaryIssues { get; }
        public ITrackingRepository<SalaryPayment> SalaryPayments { get; }

        public UnitOfWork(KhataContext context,
            ITrackingRepository<Product> products,
            ITrackingRepository<Service> services,
            ITrackingRepository<Customer> customers,
            ITrackingRepository<DebtPayment> debtPayments,
            ITrackingRepository<Sale> sales,
            ITrackingRepository<Expense> expenses,
            ITrackingRepository<Supplier> suppliers,
            ITrackingRepository<SupplierPayment> supplierPayments,
            ITrackingRepository<Purchase> purchases,
            ITrackingRepository<Employee> employees,
            ITrackingRepository<SalaryIssue> salaryIssues,
            ITrackingRepository<SalaryPayment> salaryPayments)
        {
            Context = context;
            Products = products;
            Services = services;
            Customers = customers;
            DebtPayments = debtPayments;
            Sales = sales;
            Expenses = expenses;
            Suppliers = suppliers;
            SupplierPayments = supplierPayments;
            Purchases = purchases;
            Employees = employees;
            SalaryIssues = salaryIssues;
            SalaryPayments = salaryPayments;
        }

        public void Complete()
        {
            Context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}