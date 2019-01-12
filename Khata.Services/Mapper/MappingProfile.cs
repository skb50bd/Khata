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
                    dest => dest.Modifier,
                    opt => opt.MapFrom(
                        src => src.Metadata.Modifier)
                )
                .ForMember(
                    dest => dest.Modified,
                    opt => opt.MapFrom(
                        src => src.Metadata.ModificationTime)
                )
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
            CreateMap<Service, ServiceViewModel>();
            CreateMap<ServiceViewModel, Service>();
            CreateMap<Service, ServiceDto>()
                .ForMember(
                    dest => dest.Modifier,
                    opt => opt.MapFrom(
                        src => src.Metadata.Modifier)
                )
                .ForMember(
                    dest => dest.ModificationTime,
                    opt => opt.MapFrom(
                        src => src.Metadata.ModificationTime)
                );
            CreateMap<ServiceDto, ServiceViewModel>();
            #endregion

            #region Customer Mapping
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Customer, CustomerDto>()
               .ForMember(
                    dest => dest.MetadataModifier,
                    opt => opt.MapFrom(
                        src => src.Metadata.Modifier))
              .ForMember(
                   dest => dest.MetadataModificationTime,
                   opt => opt.MapFrom(
                       src => src.Metadata.ModificationTime));
            CreateMap<CustomerDto, CustomerViewModel>();
            #endregion

            #region Debt Payment

            CreateMap<DebtPaymentViewModel, DebtPayment>();
            CreateMap<DebtPayment, DebtPaymentDto>()
               .ForMember(
                    dest => dest.MetadataModifier,
                    opt => opt.MapFrom(src => src.Metadata.Modifier)
                    )
               .ForMember(dest => dest.MetadataModificationTime,
                    opt => opt.MapFrom(src => src.Metadata.ModificationTime)
                    );
            CreateMap<DebtPaymentDto, DebtPaymentViewModel>();

            #endregion

            #region Sale

            CreateMap<Sale, SaleDto>();
            CreateMap<SaleViewModel, Sale>();
            CreateMap<SaleDto, SaleViewModel>();

            #endregion

            #region Expense

            CreateMap<ExpenseViewModel, Expense>();
            CreateMap<Expense, ExpenseDto>()
                .ForMember(
                    dest => dest.MetadataModification,
                    opt => opt.MapFrom(src => src.Metadata.ModificationTime)
                    );
            CreateMap<ExpenseDto, ExpenseViewModel>();

            #endregion
        }
    }
}