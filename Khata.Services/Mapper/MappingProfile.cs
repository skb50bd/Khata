using System;
using System.Globalization;
using System.Linq;

using AutoMapper;

using Khata.Domain;
using Khata.DTOs;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Cash Register Mapping

            CreateMap<CashRegisterViewModel, CashRegister>();
            CreateMap<CashRegister, CashRegisterDto>();
            CreateMap<CashRegisterDto, CashRegisterViewModel>();

            #endregion

            #region Outlet Mapping

            CreateMap<OutletViewModel, Outlet>();
            CreateMap<Outlet, OutletDto>();
            CreateMap<OutletDto, OutletViewModel>();

            #endregion

            #region Product Mapping
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>()
                .ForMember(
                    dest => dest.Inventory,
                    opt => opt.MapFrom(
                        src => new Inventory
                        {
                            Stock = src.InventoryStock,
                            Warehouse = src.InventoryWarehouse,
                            AlertAt = src.InventoryAlertAt
                        })
                )
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(
                        src => new Pricing
                        {
                            Purchase = src.PricePurchase,
                            Margin = src.PriceMargin,
                            Bulk = src.PriceBulk,
                            Retail = src.PriceRetail
                        })
                );

            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.InventoryStock,
                    opt => opt.MapFrom(
                        src => src.Inventory.Stock)
                )
                .ForMember(
                    dest => dest.InventoryWarehouse,
                    opt => opt.MapFrom(
                        src => src.Inventory.Warehouse)
                )
                .ForMember(
                    dest => dest.InventoryTotalStock,
                    opt => opt.MapFrom(
                        src => src.Inventory.TotalStock)
                )
                .ForMember(
                    dest => dest.InventoryStockStatus,
                    opt => opt.MapFrom(
                        src => src.Inventory.Status)
                );
            CreateMap<ProductDto, ProductViewModel>();
            #endregion

            #region Service Mapping
            CreateMap<ServiceViewModel, Service>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, ServiceViewModel>();
            #endregion

            #region Person Mapping

            CreateMap<PersonViewModel, Person>();
            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, PersonViewModel>();

            #endregion

            #region Customer Mapping
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, CustomerViewModel>();
            #endregion

            #region Transactions Mapping

            CreateMap<DepositViewModel, Deposit>();
            CreateMap<WithdrawalViewModel, Withdrawal>();

            #endregion

            #region Debt Payment Mapping

            CreateMap<DebtPaymentViewModel, DebtPayment>();
            CreateMap<DebtPayment, DebtPaymentDto>();
            CreateMap<DebtPaymentDto, DebtPaymentViewModel>();

            #endregion

            #region Sale Mapping

            CreateMap<LineItemViewModel, SaleLineItem>();
            CreateMap<SaleLineItem, LineItemViewModel>();
            CreateMap<PaymentInfoViewModel, PaymentInfo>();

            CreateMap<Sale, SaleDto>();
            CreateMap<SaleViewModel, Sale>()
                .ForMember(
                    dest => dest.SaleDate,
                    opt => opt.MapFrom(
                        src => src.SaleDate.ParseDate()
                   )
                );
            CreateMap<SaleDto, SaleViewModel>().ForMember(
                dest => dest.SaleDate,
                opt => opt.MapFrom(src => src.SaleDate.ToString("dd/MM/yyyy"))
            );

            CreateMap<SaleLineItem, InvoiceLineItem>();

            CreateMap<SaleViewModel, CustomerInvoice>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.Cart,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.PreviousDue,
                opt => opt.MapFrom(src => src.Customer.Debt)
            )
            .ForMember(
                dest => dest.PaymentSubtotal,
                opt => opt.MapFrom(src => src.Cart.Sum(li => li.NetPrice))
            );

            CreateMap<Sale, CustomerInvoice>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => src.SaleDate)
            )
            .ForMember(
                dest => dest.PreviousDue,
                opt => opt.MapFrom(src => src.Customer.Debt)
            )
            .ForMember(
                dest => dest.Sale,
                opt => opt.MapFrom(src => src)
            );

            CreateMap<DebtPayment, CustomerInvoice>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.PreviousDue,
                opt => opt.MapFrom(src => src.DebtBefore)
            )
            .ForMember(
                dest => dest.PaymentPaid,
                opt => opt.MapFrom(src => src.Amount)
            )
            .ForMember(
                dest => dest.PaymentSubtotal,
                opt => opt.MapFrom(src => 0)
            )
            .ForMember(
                dest => dest.PaymentDiscountCash,
                opt => opt.MapFrom(src => 0)
            )
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => DateTime.Now)
            )
            .ForMember(
                dest => dest.DebtPayment,
                opt => opt.MapFrom(src => src)
            );
            #endregion

            #region Purchase 

            CreateMap<LineItemViewModel, PurchaseLineItem>()
                .ForMember(
                    dest => dest.UnitPurchasePrice,
                    opt => opt.MapFrom(src => src.NetPrice / src.Quantity)
                )
                .ForMember(
                    dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.ItemId)
                );
            CreateMap<PurchaseLineItem, LineItemViewModel>()
            .ForMember(
                    dest => dest.NetPrice,
                    opt => opt.MapFrom(src => src.NetPurchasePrice)
            )
            .ForMember(
                dest => dest.ItemId,
                opt => opt.MapFrom(src => src.ProductId)
            )
            .ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(src => LineItemType.Product)
            );

            CreateMap<PurchaseLineItem, InvoiceLineItem>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPurchasePrice))
                .ForMember(dest => dest.NetPrice, opt => opt.MapFrom(src => src.NetPurchasePrice));

            CreateMap<Purchase, PurchaseDto>();
            CreateMap<PurchaseViewModel, Purchase>()
                .ForMember(
                    dest => dest.PurchaseDate,
                    opt => opt.MapFrom(
                        src => DateTimeOffset.ParseExact(
                            src.PurchaseDate,
                            @"dd/MM/yyyy",
                            CultureInfo.InvariantCulture.DateTimeFormat
                        )
                   )
                );

            CreateMap<PurchaseDto, PurchaseViewModel>().ForMember(
                dest => dest.PurchaseDate,
                opt => opt.MapFrom(src => src.PurchaseDate.ToString("dd/MM/yyyy"))
            );

            CreateMap<Purchase, Vouchar>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(src => src.PurchaseDate)
            )
            .ForMember(
                dest => dest.PreviousDue,
                opt => opt.MapFrom(src => src.Supplier.Payable)
            )
            .ForMember(
                dest => dest.Purchase,
                opt => opt.MapFrom(src => src)
            );

            CreateMap<SupplierPayment, Vouchar>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            )
            .ForMember(
                dest => dest.PreviousDue,
                opt => opt.MapFrom(src => src.PayableBefore)
            )
            .ForMember(
                dest => dest.PaymentPaid,
                opt => opt.MapFrom(src => src.Amount)
            )
            .ForMember(
                dest => dest.PaymentSubtotal,
                opt => opt.MapFrom(src => 0)
            )
            .ForMember(
                dest => dest.SupplierPayment,
                opt => opt.MapFrom(src => src)
            );
            #endregion

            #region Expense Mapping

            CreateMap<ExpenseViewModel, Expense>();
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseDto, ExpenseViewModel>();

            #endregion

            #region Supplier Mapping

            CreateMap<SupplierViewModel, Supplier>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierDto, SupplierViewModel>();

            #endregion

            #region Supplier Payment Mapping

            CreateMap<SupplierPaymentViewModel, SupplierPayment>();
            CreateMap<SupplierPayment, SupplierPaymentDto>();
            CreateMap<SupplierPaymentDto, SupplierPaymentViewModel>();

            #endregion

            #region Employee Mapping

            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, EmployeeViewModel>();

            #endregion

            #region Salary Issue and Payment Mapping

            CreateMap<SalaryIssueViewModel, SalaryIssue>();
            CreateMap<SalaryIssue, SalaryIssueDto>();
            CreateMap<SalaryIssueDto, SalaryIssueViewModel>();

            CreateMap<SalaryPaymentViewModel, SalaryPayment>();
            CreateMap<SalaryPayment, SalaryPaymentDto>();
            CreateMap<SalaryPaymentDto, SalaryPaymentViewModel>();

            #endregion

            #region Invoice / Vouchar Mapping

            CreateMap<InvoiceLineItem, InvoiceLineItemDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<CustomerInvoice, CustomerInvoiceDto>();
            CreateMap<Vouchar, VoucharDto>();

            #endregion

            #region Refund / Purchase Return Mapping

            CreateMap<Refund, RefundDto>();
            CreateMap<RefundViewModel, Refund>();

            CreateMap<PurchaseReturn, PurchaseReturnDto>();
            CreateMap<PurchaseReturnViewModel, PurchaseReturn>();

            #endregion
        }
    }
}