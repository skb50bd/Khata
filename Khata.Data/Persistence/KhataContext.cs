
using Khata.Domain;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public class KhataContext : IdentityDbContext
    {
        public KhataContext(DbContextOptions<KhataContext> options) : base(options)
        {
            //IHostingEnvironment env
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.OwnsOne(p => p.Metadata);

                entity.OwnsOne(p => p.Price);

                entity.OwnsOne(p => p.Inventory);
            });

            modelBuilder.Entity<Service>()
                        .OwnsOne(s => s.Metadata);

            modelBuilder.Entity<Customer>()
                        .OwnsOne(c => c.Metadata);

            modelBuilder.Entity<DebtPayment>(entity =>
            {
                entity.Property(d => d.CustomerId).IsRequired();
                entity.OwnsOne(d => d.Metadata);
                entity.HasOne(dp => dp.Invoice)
                    .WithOne(i => i.DebtPayment)
                    .HasForeignKey<Invoice>(i => i.DebtPaymentId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<SaleLineItem>(entity =>
            {
                entity.HasKey(li => li.Id);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.OwnsOne(s => s.Payment);
                entity.OwnsOne(s => s.Metadata);
                entity.HasOne(s => s.Invoice)
                    .WithOne(i => i.Sale)
                    .HasForeignKey<Invoice>(i => i.SaleId)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(i => i.Sale).WithOne(s => s.Invoice).HasForeignKey<Sale>(s => s.InvoiceId);
                entity.HasOne(i => i.DebtPayment).WithOne(s => s.Invoice).HasForeignKey<DebtPayment>(s => s.InvoiceId);
                entity.OwnsOne(s => s.Metadata);
                entity.OwnsMany(s => s.Cart).HasKey(s => s.Id);
            });

            modelBuilder.Entity<Expense>().OwnsOne(e => e.Metadata);

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.OwnsOne(s => s.Metadata);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.OwnsOne(s => s.Payment);
                entity.OwnsOne(s => s.Metadata);
            });

            modelBuilder.Entity<SupplierPayment>(entity =>
            {
                entity.Property(sp => sp.SupplierId).IsRequired();
                entity.OwnsOne(sp => sp.Metadata);
            });

            modelBuilder.Entity<Employee>().OwnsOne(e => e.Metadata);

            modelBuilder.Entity<SalaryIssue>(entity =>
            {
                entity.Property(si => si.EmployeeId).IsRequired();
                entity.OwnsOne(si => si.Metadata);
            });

            modelBuilder.Entity<SalaryPayment>(entity =>
            {
                entity.Property(sp => sp.EmployeeId).IsRequired();
                entity.OwnsOne(sp => sp.Metadata);
            });

            modelBuilder.Entity<CashRegister>().OwnsOne(cr => cr.Metadata);

            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.OwnsOne(d => d.Metadata);
            });
            modelBuilder.Entity<Withdrawal>(entity =>
            {
                entity.OwnsOne(d => d.Metadata);
            });
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
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierPayment> SupplierPayments { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<SalaryIssue> SalaryIssues { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }

    }
}