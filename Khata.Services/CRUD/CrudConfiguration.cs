using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.CRUD
{
    public static class CrudConfiguration
    {
        public static IServiceCollection ConfigureCrudServices(
            this IServiceCollection services)
        {
            services.AddTransient<ICashRegisterService, CashRegisterService>();
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
            return services;
        }
    }
}
