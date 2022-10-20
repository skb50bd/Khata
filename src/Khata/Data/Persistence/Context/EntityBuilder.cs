using System.Linq;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence;

public static class EntityBuilder
{
    public static ModelBuilder BuildEntities(this ModelBuilder builder)
    {
        builder.Owned<Pricing>();
        builder.Owned<Inventory>();
        builder.Owned<PaymentInfo>();

        builder.Entity<DebtPayment>(entity =>
        {
            entity.Property(d => d.CustomerId).IsRequired();
            entity.HasOne(dp => dp.Invoice)
                .WithOne(i => i.DebtPayment)
                .HasForeignKey<CustomerInvoice>(i => i.DebtPaymentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        builder.Entity<SaleLineItem>(entity =>
        {
            entity.HasKey(li => li.Id);
        });

        builder.Entity<Sale>(entity =>
        {
            entity.HasOne(s => s.Invoice)
                .WithOne(i => i.Sale)
                .HasForeignKey<CustomerInvoice>(i => i.SaleId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        builder.Entity<CustomerInvoice>(entity =>
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

        builder.Entity<Purchase>(entity =>
        {
            entity.HasOne(p => p.Vouchar)
                .WithOne(v => v.Purchase)
                .HasForeignKey<Vouchar>(v => v.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        builder.Entity<SupplierPayment>(entity =>
        {
            entity.Property(sp => sp.SupplierId).IsRequired();
            entity.HasOne(sp => sp.Vouchar)
                .WithOne(v => v.SupplierPayment)
                .HasForeignKey<Vouchar>(v => v.SupplierPaymentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        builder.Entity<Vouchar>(entity =>
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

        builder.Entity<SalaryIssue>(entity =>
        {
            entity.Property(si => si.EmployeeId).IsRequired();
        });

        builder.Entity<SalaryPayment>(entity =>
        {
            entity.Property(sp => sp.EmployeeId).IsRequired();
        });

        builder.Entity<Refund>(entity =>
        {
            entity.HasOne(r => r.Sale)
                .WithOne()
                .HasForeignKey<Refund>(r => r.SaleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<PurchaseReturn>(entity =>
        {
            entity.HasOne(r => r.Purchase)
                .WithOne()
                .HasForeignKey<PurchaseReturn>(r => r.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Outlet>(entity =>
        {
            entity.HasMany(e => e.Sales)
                .WithOne(s => s.Outlet)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Products)
                .WithOne(s => s.Outlet)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Services)
                .WithOne(s => s.Outlet).OnDelete(DeleteBehavior.Restrict);
        });

        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal)))
        {
            property.SetColumnType("decimal(18, 6)");
        }

        var metas =
            builder.Model.GetEntityTypes().SelectMany(
                    d => d.GetNavigations())
                .Where(p => p.Name == nameof(Document.Metadata));

        foreach (var p in metas)
            p.SetIsEagerLoaded(true);

        return builder;
    }
}