using System;
using System.Linq;

namespace WebUI
{
    public static class AreaHelper
    {
        public static string FindArea(string section)
        {

            var cash = new[] { "Cash" };
            var dashboard = new[] { "Dashboard" };
            var inventory = new[]{
                "Products",
                "Services"
            };
            var incoming = new[] {
                "Sales",
                "DebtPayments",
                "PurchaseReturns",
                "Deposits"
            };
            var outgoing = new[] {
                "Expenses",
                "Purchases",
                "SupplierPayments",
                "SalaryPayments",
                "SalaryIssues",
                "Refunds",
                "Withdrawals"
            };
            var outlets = new[] { "Outlets" };
            var people = new[]{
                "Customers",
                "Suppliers",
                "Employees"
            };
            var reporting = new[] { "Reporting" };

            if (cash.Contains(section)) return "Cash";
            if (dashboard.Contains(section)) return "Dashboard";
            if (inventory.Contains(section)) return "Inventory";
            if (incoming.Contains(section)) return "Incoming";
            if (outgoing.Contains(section)) return "Outgoing";
            if (outlets.Contains(section)) return "Outlets";
            if (people.Contains(section)) return "People";
            if (reporting.Contains(section)) return "Reporting";
            return string.Empty;
        }
    }
}
