using System.Linq;

using Khata.Domain;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public static class EntityBuilder
    {
        public static ModelBuilder BuildEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<Pricing>();
            modelBuilder.Owned<Inventory>();
            modelBuilder.Owned<PaymentInfo>();

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

            foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal)))
                    {
                        property.Relational().ColumnType = "decimal(18, 6)";
                    }

            var metas =
                modelBuilder.Model.GetEntityTypes().SelectMany(
                    d => d.GetNavigations())
                        .Where(p => p.Name == nameof(Document.Metadata));

            foreach (var p in metas)
                p.IsEagerLoaded = true;

            return modelBuilder;
        }
    }
}
