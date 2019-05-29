using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public sealed partial class KhataContext
    {
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<CashRegister> CashRegister { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DebtPayment> DebtPayments { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SavedSale> SavedSales { get; set; }
        public DbSet<CustomerInvoice> Invoices { get; set; }
        public DbSet<Vouchar> Vouchars { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierPayment> SupplierPayments { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryIssue> SalaryIssues { get; set; }
        public DbSet<SalaryPayment> SalaryPayments { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
    }
}
