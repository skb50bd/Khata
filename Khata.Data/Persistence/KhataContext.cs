
using System.Linq;

using Khata.Domain;
using Khata.Queries;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public class KhataContext : IdentityDbContext
    {
        public KhataContext(DbContextOptions<KhataContext> options) : base(options)
        {
            var pm = Database.GetPendingMigrations();
            if (pm.Any())
            {
                Database.Migrate();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Owned<Metadata>();
            modelBuilder.Owned<Pricing>();
            modelBuilder.Owned<Inventory>();
            modelBuilder.Owned<PaymentInfo>();

            modelBuilder.Entity<Metadata>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Creator).HasDefaultValue("admin");
                entity.Property(m => m.Modifier).HasDefaultValue("admin");
                entity.Property(m => m.CreationTime).HasDefaultValueSql("GETDATE()");
                entity.Property(m => m.ModificationTime).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<DebtPayment>(entity =>
            {
                entity.Property(d => d.CustomerId).IsRequired();
                entity.HasOne(dp => dp.Invoice)
                    .WithOne(i => i.DebtPayment)
                    .HasForeignKey<CustomerInvoice>(i => i.DebtPaymentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);
            });

            modelBuilder.Entity<SaleLineItem>(entity =>
            {
                entity.HasKey(li => li.Id);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(s => s.Invoice)
                        .WithOne(i => i.Sale)
                        .HasForeignKey<CustomerInvoice>(i => i.SaleId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired(false);
            });

            modelBuilder.Entity<CustomerInvoice>(entity =>
            {
                entity.HasBaseType<Invoice>();
                entity.HasOne(i => i.Sale)
                        .WithOne(s => s.Invoice)
                        .HasForeignKey<Sale>(s => s.InvoiceId);
                entity.HasOne(i => i.DebtPayment)
                        .WithOne(s => s.Invoice)
                        .HasForeignKey<DebtPayment>(s => s.InvoiceId);
                entity.OwnsMany(s => s.Cart)
                        .HasKey(s => s.Id);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasOne(p => p.Vouchar)
                        .WithOne(v => v.Purchase)
                        .HasForeignKey<Vouchar>(v => v.PurchaseId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired(false);
            });

            modelBuilder.Entity<SupplierPayment>(entity =>
            {
                entity.Property(sp => sp.SupplierId).IsRequired();
                entity.HasOne(sp => sp.Vouchar)
                        .WithOne(v => v.SupplierPayment)
                        .HasForeignKey<Vouchar>(v => v.SupplierPaymentId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired(false);
            });

            modelBuilder.Entity<Vouchar>(entity =>
            {
                entity.HasBaseType<Invoice>();
                entity.HasOne(v => v.Purchase)
                        .WithOne(p => p.Vouchar)
                        .HasForeignKey<Purchase>(p => p.VoucharId);
                entity.HasOne(v => v.SupplierPayment)
                        .WithOne(s => s.Vouchar)
                        .HasForeignKey<SupplierPayment>(sp => sp.VoucharId);
                entity.OwnsMany(s => s.Cart)
                        .HasKey(s => s.Id);
            });

            modelBuilder.Entity<SalaryIssue>(entity =>
            {
                entity.Property(si => si.EmployeeId).IsRequired();
            });

            modelBuilder.Entity<SalaryPayment>(entity =>
            {
                entity.Property(sp => sp.EmployeeId).IsRequired();
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasOne(r => r.Sale)
                        .WithOne()
                        .HasForeignKey<Refund>(r => r.SaleId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PurchaseReturn>(entity =>
            {
                entity.HasOne(r => r.Purchase)
                        .WithOne()
                        .HasForeignKey<PurchaseReturn>(r => r.PurchaseId)
                        .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Query<CustomerReport>().ToQuery(() =>
                Customers.Select(c => new CustomerReport
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Address = c.Address,
                    Debt = c.Debt,
                    SalesCount = c.Purchases.Count(),
                    SaleReceives = c.Purchases.Sum(s => s.Payment.Paid),
                    SalesWorth = c.Purchases.Sum(s => s.Payment.Total),
                    Profit = c.Purchases.Sum(s => s.Profit),

                    DebtPaymentsCount = c.DebtPayments.Count(),
                    DebtPaymentReceives = c.DebtPayments.Sum(d => d.Amount),

                    RefundCount = c.Refunds.Count(),
                    RefundLoss = c.Refunds.Sum(r => r.EffectiveLoss),
                    RefundAmount = c.Refunds.Sum(r => r.TotalBackPaid)
                })
            );

            modelBuilder.Query<SupplierReport>().ToQuery(() =>
                Suppliers.Select(c => new SupplierReport
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Address = c.Address,
                    Payable = c.Payable,
                    PurchasesCount = c.Purchases.Count(),
                    PurchasePaid = c.Purchases.Sum(s => s.Payment.Paid),
                    PurchasesWorth = c.Purchases.Sum(s => s.Payment.Total),

                    SupplierPaymentsCount = c.Payments.Count(),
                    SupplierPaymentPaid = c.Payments.Sum(d => d.Amount),

                    PurchaseReturnCount = c.PurchaseReturns.Count(),
                    PurchaseReturnAmount = c.PurchaseReturns.Sum(r => r.TotalBackPaid)
                })
            );
        }

        public virtual DbSet<CashRegister> CashRegister { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<Withdrawal> Withdrawals { get; set; }
        public virtual DbSet<ApplicationUser> AppUsers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DebtPayment> DebtPayments { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Sale> SavedSales { get; set; }

        public virtual DbSet<CustomerInvoice> Invoices { get; set; }
        public virtual DbSet<Vouchar> Vouchars { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierPayment> SupplierPayments { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<SalaryIssue> SalaryIssues { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<PurchaseReturn> PurchaseReturns { get; set; }

        public virtual DbQuery<CustomerReport> CustomerReports { get; set; }
        public virtual DbQuery<SupplierReport> SupplierReports { get; set; }

    }
}