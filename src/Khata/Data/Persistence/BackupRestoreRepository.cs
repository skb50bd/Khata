using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;

using Data.Core;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Data.Persistence
{
    public class BackupRestoreRepository 
        : IBackupRestoreRepository
    {
        private readonly KhataContext _ctx;

        public BackupRestoreRepository(
            KhataContext ctx) =>
            _ctx = ctx;

        public async Task<Stream> GetJsonDump()
        {
            #region Get the Data From Database
            var users = await _ctx.AppUsers.OrderBy(e => e.Id).ToListAsync();

            var outlets = await _ctx.Outlets.OrderBy(e => e.Id).ToListAsync();

            var cashRegisters = await _ctx.CashRegister.OrderBy(e => e.Id).ToListAsync();
            
            var products = await _ctx.Products.OrderBy(e => e.Id).ToListAsync();
            
            var services = await _ctx.Services.OrderBy(e => e.Id).ToListAsync();
            
            var savedSales = await _ctx.SavedSales.OrderBy(e => e.Id)
                                         .Include(ss => ss.Cart)
                                         .ToListAsync();
            
            var customers = await _ctx.Customers.OrderBy(e => e.Id).ToListAsync();
            
            var debtPayments = await _ctx.DebtPayments.OrderBy(e => e.Id).ToListAsync();
            
            var sales = await _ctx.Sales.OrderBy(e => e.Id)
                                 .Include(s => s.Cart)
                                 .ToListAsync();
            
            var invoices = await _ctx.Invoices.OrderBy(e => e.Id)
                                         .Include(i => i.Cart)
                                         .ToListAsync();
            
            var refunds = await _ctx.Refunds.OrderBy(e => e.Id)
                                     .Include(r => r.Cart)
                                     .ToListAsync();
            
            var suppliers = await _ctx.Suppliers.OrderBy(e => e.Id).ToListAsync();
            
            var supplierPayments = await _ctx.SupplierPayments.OrderBy(e => e.Id).ToListAsync();
            
            var purchases = await _ctx.Purchases.OrderBy(e => e.Id)
                                         .Include(p => p.Cart)
                                         .ToListAsync();
            
            var vouchars = await _ctx.Vouchars.OrderBy(e => e.Id)
                                         .Include(v => v.Cart)
                                         .ToListAsync();
            
            var purchaseReturns = await _ctx.PurchaseReturns.OrderBy(e => e.Id)
                                             .Include(pr => pr.Cart)
                                             .ToListAsync();
            
            var employees = await _ctx.Employees.OrderBy(e => e.Id).ToListAsync();
            
            var salaryIssues = await _ctx.SalaryIssues.OrderBy(e => e.Id).ToListAsync();
            
            var salaryPayments = await _ctx.SalaryPayments.OrderBy(e => e.Id).ToListAsync();
            
            var expenses = await _ctx.Expenses.OrderBy(e => e.Id).ToListAsync();
            
            var deposits = await _ctx.Deposits.OrderBy(e => e.Id).ToListAsync();
            
            var withdrawals = await _ctx.Withdrawals.OrderBy(e => e.Id).ToListAsync();
            #endregion

            #region Nullify Navigation Properties (to Avoid Reference Looping)
            outlets.ForEach(o =>
                {
                    o.Products = null;
                    o.Services = null;
                    o.Sales = null;
                });

            customers.ForEach(c =>
            {
                c.Purchases = null;
                c.DebtPayments = null;
                c.Refunds = null;
            });

            debtPayments.ForEach(dp =>
            {
                dp.Customer = null;
                dp.Invoice = null;
            });

            sales.ForEach(s =>
            {
                s.Customer = null;
                s.Invoice = null;
                s.Outlet = null;
            });

            invoices.ForEach(i =>
            {
                i.Customer = null;
                i.Outlet = null;
                i.Sale = null;
                i.DebtPayment = null;
            });

            refunds.ForEach(r =>
            {
                r.Customer = null;
                r.Sale = null;
            });

            suppliers.ForEach(s =>
            {
                s.Purchases = null;
                s.PurchaseReturns = null;
                s.Payments = null;
            });

            supplierPayments.ForEach(sp =>
            {
                sp.Supplier = null;
                sp.Vouchar = null;
            });

            purchases.ForEach(p =>
            {
                p.Supplier = null;
                p.Vouchar = null;
            });

            vouchars.ForEach(v =>
            {
                v.Purchase = null;
                v.Supplier = null;
                v.SupplierPayment = null;
            });

            purchaseReturns.ForEach(pr =>
            {
                pr.Supplier = null;
                pr.Purchase = null;
            });

            employees.ForEach(e =>
            {
                e.SalaryIssues = null;
                e.SalaryPayments = null;
            });

            salaryIssues.ForEach(si =>
            {
                si.Employee = null;
            });

            salaryPayments.ForEach(sp =>
            {
                sp.Employee = null;
            });

            #endregion

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling     = NullValueHandling.Ignore,
                Formatting            = Formatting.Indented
            };

            var items = new List<ZipItem>
            {
                new ZipItem(
                    $"{nameof(users).Capitalize()}.json",
                    JsonConvert.SerializeObject(users, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(outlets).Capitalize()}.json",
                    JsonConvert.SerializeObject(outlets, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(cashRegisters).Capitalize()}.json",
                    JsonConvert.SerializeObject(cashRegisters, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(products).Capitalize()}.json",
                    JsonConvert.SerializeObject(products, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(services).Capitalize()}.json",
                    JsonConvert.SerializeObject(services, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(savedSales).Capitalize()}.json",
                    JsonConvert.SerializeObject(savedSales, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(customers).Capitalize()}.json",
                    JsonConvert.SerializeObject(customers, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(debtPayments).Capitalize()}.json",
                    JsonConvert.SerializeObject(debtPayments, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(sales).Capitalize()}.json",
                    JsonConvert.SerializeObject(sales, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(invoices).Capitalize()}.json",
                    JsonConvert.SerializeObject(invoices, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(refunds).Capitalize()}.json",
                    JsonConvert.SerializeObject(refunds, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(suppliers).Capitalize()}.json",
                    JsonConvert.SerializeObject(suppliers, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(supplierPayments).Capitalize()}.json",
                    JsonConvert.SerializeObject(supplierPayments, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(purchases).Capitalize()}.json",
                    JsonConvert.SerializeObject(purchases, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(vouchars).Capitalize()}.json",
                    JsonConvert.SerializeObject(vouchars, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(purchaseReturns).Capitalize()}.json",
                    JsonConvert.SerializeObject(purchaseReturns, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(employees).Capitalize()}.json",
                    JsonConvert.SerializeObject(employees, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(salaryIssues).Capitalize()}.json",
                    JsonConvert.SerializeObject(salaryIssues, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(salaryPayments).Capitalize()}.json",
                    JsonConvert.SerializeObject(salaryPayments, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(expenses).Capitalize()}.json",
                    JsonConvert.SerializeObject(expenses, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(deposits).Capitalize()}.json",
                    JsonConvert.SerializeObject(deposits, settings),
                    Encoding.Unicode
                ),
                new ZipItem(
                    $"{nameof(withdrawals).Capitalize()}.json",
                    JsonConvert.SerializeObject(withdrawals, settings),
                    Encoding.Unicode
                )
            };

            return Zipper.Zip(items);
        }

        public Task<bool> RestoreFromJson(string dump)
        {
            throw new System.NotImplementedException();
        }
    }
}
