using AutoMapper;

using Khata.Domain;
using Khata.Domain.ViewModels;
using Khata.DTOs;

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
                    opt => opt.MapFrom(src => new Inventory
                    {
                        Stock = src.InventoryStock,
                        Godown = src.InventoryGodown,
                        AlertAt = src.InventoryAlertAt
                    })
                )
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => new Pricing
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
                    opt => opt.MapFrom(src => src.Metadata.Modifier)
                )
                .ForMember(
                    dest => dest.Stock,
                    opt => opt.MapFrom(src => src.Inventory.Stock)
                )
                .ForMember(
                    dest => dest.Godown,
                    opt => opt.MapFrom(src => src.Inventory.Godown)
                )
                .ForMember(
                    dest => dest.TotalStock,
                    opt => opt.MapFrom(src => src.Inventory.TotalStock)
                )
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Inventory.Status)
                );
            #endregion
        }
    }
}