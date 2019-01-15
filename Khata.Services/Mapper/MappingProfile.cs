using AutoMapper;

using Khata.Domain;
using Khata.DTOs;
using Khata.ViewModels;

namespace Khata.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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

            #region Debt Payment Mapping

            CreateMap<DebtPaymentViewModel, DebtPayment>();
            CreateMap<DebtPayment, DebtPaymentDto>();
            CreateMap<DebtPaymentDto, DebtPaymentViewModel>();

            #endregion

            #region Sale Mapping

            CreateMap<Sale, SaleDto>();
            CreateMap<SaleViewModel, Sale>();
            CreateMap<SaleDto, SaleViewModel>();

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
        }
    }
}