using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.CRUD
{
    public static class CrudConfiguration
    {
        public static IServiceCollection ConfigureCrudServices(
            this IServiceCollection services)
        {
            services.AddScoped<ICashRegisterService, CashRegisterService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IDebtPaymentService, DebtPaymentService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierPaymentService, SupplierPaymentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISalaryIssueService, SalaryIssueService>();
            services.AddScoped<ISalaryPaymentService, SalaryPaymentService>();
            return services;
        }
    }
}
