﻿// <auto-generated />
using System;
using Khata.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Khata.Data.Persistence.Migrations
{
    [DbContext(typeof(KhataContext))]
    partial class KhataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Khata.Domain.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("CompanyName");

                    b.Property<decimal>("Debt");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("LastName");

                    b.Property<string>("Note");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Khata.Domain.DebtPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<int>("CustomerId");

                    b.Property<decimal>("DebtBefore");

                    b.Property<bool>("IsRemoved");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("DebtPayments");
                });

            modelBuilder.Entity("Khata.Domain.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Description");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Khata.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Khata.Domain.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId");

                    b.Property<bool>("IsRemoved");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Khata.Domain.SaleLineItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ItemId");

                    b.Property<string>("Name");

                    b.Property<decimal>("Quantity");

                    b.Property<int?>("SaleId");

                    b.Property<int>("Type");

                    b.Property<decimal>("UnitPrice");

                    b.Property<decimal>("UnitPurchasePrice");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleLineItem");
                });

            modelBuilder.Entity("Khata.Domain.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Khata.Domain.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<byte[]>("Avatar");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("Role");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Khata.Domain.Customer", b =>
                {
                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("Customers");

                            b1.HasOne("Khata.Domain.Customer")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Khata.Domain.DebtPayment", b =>
                {
                    b.HasOne("Khata.Domain.Customer", "Customer")
                        .WithMany("DebtPayments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("DebtPayments");

                            b1.HasOne("Khata.Domain.DebtPayment")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Khata.Domain.Expense", b =>
                {
                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("Expenses");

                            b1.HasOne("Khata.Domain.Expense")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Khata.Domain.Product", b =>
                {
                    b.OwnsOne("Khata.Domain.Inventory", "Inventory", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("AlertAt");

                            b1.Property<decimal>("Stock");

                            b1.Property<decimal>("Warehouse");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.HasOne("Khata.Domain.Product")
                                .WithOne("Inventory")
                                .HasForeignKey("Khata.Domain.Inventory", "ProductId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Khata.Domain.Pricing", "Price", b1 =>
                        {
                            b1.Property<int>("ProductId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("Bulk");

                            b1.Property<decimal>("Margin");

                            b1.Property<decimal>("Purchase");

                            b1.Property<decimal>("Retail");

                            b1.HasKey("ProductId");

                            b1.ToTable("Products");

                            b1.HasOne("Khata.Domain.Product")
                                .WithOne("Price")
                                .HasForeignKey("Khata.Domain.Pricing", "ProductId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("Products");

                            b1.HasOne("Khata.Domain.Product")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Khata.Domain.Sale", b =>
                {
                    b.HasOne("Khata.Domain.Customer", "Customer")
                        .WithMany("Purchases")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Khata.Domain.PaymentInfo", "Payment", b1 =>
                        {
                            b1.Property<int>("SaleId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("DiscountCash");

                            b1.Property<decimal>("Paid");

                            b1.Property<decimal>("SubTotal");

                            b1.HasKey("SaleId");

                            b1.ToTable("Sales");

                            b1.HasOne("Khata.Domain.Sale")
                                .WithOne("Payment")
                                .HasForeignKey("Khata.Domain.PaymentInfo", "SaleId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("Sales");

                            b1.HasOne("Khata.Domain.Sale")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Khata.Domain.SaleLineItem", b =>
                {
                    b.HasOne("Khata.Domain.Sale")
                        .WithMany("Cart")
                        .HasForeignKey("SaleId");
                });

            modelBuilder.Entity("Khata.Domain.Service", b =>
                {
                    b.OwnsOne("Khata.Domain.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTimeOffset>("CreationTime");

                            b1.Property<string>("Creator");

                            b1.Property<DateTimeOffset>("ModificationTime");

                            b1.Property<string>("Modifier");

                            b1.HasKey("Id");

                            b1.ToTable("Services");

                            b1.HasOne("Khata.Domain.Service")
                                .WithOne("Metadata")
                                .HasForeignKey("Khata.Domain.Metadata", "Id")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
