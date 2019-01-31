﻿using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface IUnitOfWork
    {
        ICashRegisterRepository CashRegister { get; }
        IRepository<Deposit> Deposits { get; }
        IRepository<Withdrawal> Withdrawals { get; }
        ITrackingRepository<Product> Products { get; }
        ITrackingRepository<Service> Services { get; }
        ITrackingRepository<Customer> Customers { get; }
        ITrackingRepository<Sale> Sales { get; }
        ITrackingRepository<CustomerInvoice> Invoices { get; }
        ITrackingRepository<Vouchar> Vouchars { get; }
        ITrackingRepository<DebtPayment> DebtPayments { get; }
        ITrackingRepository<Expense> Expenses { get; }
        ITrackingRepository<Supplier> Suppliers { get; }
        ITrackingRepository<SupplierPayment> SupplierPayments { get; }
        ITrackingRepository<Purchase> Purchases { get; }
        ITrackingRepository<Employee> Employees { get; }
        ITrackingRepository<SalaryIssue> SalaryIssues { get; }
        ITrackingRepository<SalaryPayment> SalaryPayments { get; }
        ITrackingRepository<Refund> Refunds { get; }
        void Complete();

        Task CompleteAsync();
    }
}