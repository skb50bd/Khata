using Business.Abstractions;
using Business.Email;
using Business.Implementations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureCrudServices(
            this IServiceCollection services)
        {
            services.AddTransient<ICashRegisterService, CashRegisterService>();
            services.AddTransient<IOutletService, OutletService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<ICustomerInvoiceService, InvoiceService>();
            services.AddTransient<IVoucharService, VoucharService>();
            services.AddTransient<IDebtPaymentService, DebtPaymentService>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<ISupplierPaymentService, SupplierPaymentService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ISalaryIssueService, SalaryIssueService>();
            services.AddTransient<ISalaryPaymentService, SalaryPaymentService>();
            services.AddTransient<IRefundService, RefundService>();
            services.AddTransient<IPurchaseReturnService, PurchaseReturnService>();

            services.AddTransient<BackupRestoreService>();

            services.AddTransient<IEmailSender, Sender>();
            return services;
        }
    }
}
