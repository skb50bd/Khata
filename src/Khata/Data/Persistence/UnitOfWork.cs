using System.Threading.Tasks;

using Data.Core;

using Domain;

namespace Data.Persistence;

public class UnitOfWork : IUnitOfWork
{
    #region Dependencies
    protected readonly KhataContext Context;
    public ITrackingRepository<Outlet> Outlets { get; }
    public ITrackingRepository<Product> Products { get; }
    public ITrackingRepository<Service> Services { get; }
    public ITrackingRepository<Customer> Customers { get; }
    public ITrackingRepository<DebtPayment> DebtPayments { get; }
    public ISaleRepository Sales { get; }
    public ITrackingRepository<CustomerInvoice> Invoices { get; }
    public ITrackingRepository<Vouchar> Vouchars { get; }
    public ITrackingRepository<Expense> Expenses { get; }
    public ITrackingRepository<Supplier> Suppliers { get; }
    public ITrackingRepository<SupplierPayment> SupplierPayments { get; }
    public ITrackingRepository<Purchase> Purchases { get; }
    public ITrackingRepository<Employee> Employees { get; }
    public ITrackingRepository<SalaryIssue> SalaryIssues { get; }
    public ITrackingRepository<SalaryPayment> SalaryPayments { get; }
    public ICashRegisterRepository CashRegister { get; }
    public IRepository<Deposit> Deposits { get; }
    public IRepository<Withdrawal> Withdrawals { get; }
    public ITrackingRepository<Refund> Refunds { get; }
    public ITrackingRepository<PurchaseReturn> PurchaseReturns { get; }
    #endregion

    public UnitOfWork(
        #region Injected Dependencies
        KhataContext context,
        ITrackingRepository<Outlet> outlets,
        ICashRegisterRepository cashRegister,
        IRepository<Deposit> deposits,
        IRepository<Withdrawal> withdrawals,
        ITrackingRepository<Product> products,
        ITrackingRepository<Service> services,
        ITrackingRepository<Customer> customers,
        ITrackingRepository<DebtPayment> debtPayments,
        ISaleRepository sales,
        ITrackingRepository<CustomerInvoice> invoices,
        ITrackingRepository<Vouchar> vouchars,
        ITrackingRepository<Expense> expenses,
        ITrackingRepository<Supplier> suppliers,
        ITrackingRepository<SupplierPayment> supplierPayments,
        ITrackingRepository<Purchase> purchases,
        ITrackingRepository<Employee> employees,
        ITrackingRepository<SalaryIssue> salaryIssues,
        ITrackingRepository<SalaryPayment> salaryPayments,
        ITrackingRepository<Refund> refunds,
        ITrackingRepository<PurchaseReturn> purchaseReturns
        #endregion
    )
    {
        Context          = context;
        Outlets          = outlets;
        CashRegister     = cashRegister;
        Deposits         = deposits;
        Withdrawals      = withdrawals;
        Products         = products;
        Services         = services;
        Customers        = customers;
        DebtPayments     = debtPayments;
        Sales            = sales;
        Invoices         = invoices;
        Vouchars         = vouchars;
        Expenses         = expenses;
        Suppliers        = suppliers;
        SupplierPayments = supplierPayments;
        Purchases        = purchases;
        Employees        = employees;
        SalaryIssues     = salaryIssues;
        SalaryPayments   = salaryPayments;
        Refunds          = refunds;
        PurchaseReturns  = purchaseReturns;
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