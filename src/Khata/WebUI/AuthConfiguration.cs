using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    public static class AuthConfiguration
    {
        public static IServiceCollection AddPolicies(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRights",
                    policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserRights",
                    policy => policy.RequireRole("User"));
            });

            return services;
        }

        public static IMvcBuilder AddAuthorizationDefinition(
            this IMvcBuilder mvc)
        {
            mvc.AddRazorPagesOptions(
                options =>
                {
                    #region People
                    options.Conventions.AuthorizeAreaFolder(
                                    "People",
                                    "/Customers",
                                    "AdminRights");

                    options.Conventions.AllowAnonymousToAreaPage(
                        "People",
                        "/Customers/Brief");

                    options.Conventions.AuthorizeAreaFolder(
                        "People",
                        "/Suppliers",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "People",
                        "/Employees",
                        "AdminRights");
                    #endregion

                    #region Cash
                    options.Conventions.AuthorizeAreaPage(
                                    "Cash",
                                    "/Index",
                                    "AdminRights");
                    #endregion

                    #region DashBoard
                    options.Conventions.AuthorizeAreaPage(
                        "Dashboard",
                        "/Index",
                        "AdminRights");
                    #endregion

                    #region Reporting
                    options.Conventions.AuthorizeAreaPage(
                                    "Reporting",
                                    "/Index",
                                    "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/DueReport",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/InEmptyOrNegativeStockReport",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/InLowStockReport",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/InStockReport",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/StockReport",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Reporting",
                        "/SupplierDueReport",
                        "AdminRights");
                    #endregion

                    #region Outlets
                    options.Conventions.AuthorizeAreaPage(
                        "Outlets",
                        "/Create",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Outlets",
                        "/Edit",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Outlets",
                        "/Details",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaPage(
                        "Outlets",
                        "/Index",
                        "AdminRights");
                    #endregion

                    #region Inventory
                    options.Conventions.AuthorizeAreaFolder(
                        "Inventory",
                        "/Products",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Inventory",
                        "/Services",
                        "AdminRights");
                    #endregion

                    #region Incoming
                    options.Conventions.AuthorizeAreaFolder(
                        "Incoming",
                        "/DebtPayments",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Incoming",
                        "/Deposits",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Incoming",
                        "/Invoices",
                        "UserRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Incoming",
                        "/PurchaseReturns",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Incoming",
                        "/Sales",
                        "UserRights");

                    #endregion

                    #region Outgoing
                    options.Conventions.AuthorizeAreaFolder(
                                    "Outgoing",
                                    "/Expenses",
                                    "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/Purchases",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/Refunds",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/SalaryIssues",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/SalaryPayments",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/SupplierPayments",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/Vouchars",
                        "AdminRights");

                    options.Conventions.AuthorizeAreaFolder(
                        "Outgoing",
                        "/Withdrawals",
                        "AdminRights"); 
                    #endregion
                }
            );

            return mvc;
        }
    }
}
