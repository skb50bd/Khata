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
                    dest => dest.Stock,
                    opt => opt.MapFrom(
                        src => src.Inventory.Stock)
                )
                .ForMember(
                    dest => dest.Warehouse,
                    opt => opt.MapFrom(
                        src => src.Inventory.Warehouse)
                )
                .ForMember(
                    dest => dest.TotalStock,
                    opt => opt.MapFrom(
                        src => src.Inventory.TotalStock)
                )
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(
                        src => src.Inventory.Status)
                );
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

            #endregion

            #region Sale

            CreateMap<Sale, SaleDto>();

            #endregion
        }
    }
}