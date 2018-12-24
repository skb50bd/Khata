
using AutoMapper;

using Khata.Domain;
using Khata.Domain.ViewModels;

using Xunit;

namespace Services.Tests
{
    public class MappingTests
    {
        private IMapper _mapper;
        public MappingTests(IMapper mapper)
        {
            _mapper = mapper;
        }

        private const string ProductName = "My Product";
        private const string ProductDescription = "My Product's Long Long Description";
        private const string Manufacturer = "Me Me Me";
        private const decimal InventoryAlertAt = 3M;
        private const decimal InventoryGodown = 20M;
        private const decimal InventoryStock = 10M;
        private const string ProductUnit = "Box";
        private const decimal PriceBulk = 8M;
        private const decimal PriceRetail = 10M;
        private const decimal PriceMargin = 7M;
        private const decimal PricePurchase = 5M;
        private const string Username = "adminuser";

        [Fact]
        public void ProductsViewModelShouldBeMapped()
        {
            // Arrange
            var product = new Product()
            {
                Id = 1,
                Deleted = false,
                Name = ProductName,
                Description = ProductDescription,
                Inventory = new Inventory
                {
                    AlertAt = InventoryAlertAt,
                    Godown = InventoryGodown,
                    Stock = InventoryStock,
                },
                Unit = ProductUnit,
                Price = new Pricing
                {
                    Retail = PriceRetail,
                    Bulk = PriceBulk,
                    Margin = PriceMargin,
                    Purchase = PricePurchase
                },
                Metadata = Metadata.CreatedNew(Username),
            };

            var expected = new ProductViewModel
            {
                Id = 1,
                Name = ProductName,
                Description = ProductDescription,
                PriceBulk = PriceBulk,
                PriceMargin = PriceMargin,
                PricePurchase = PricePurchase,
                PriceRetail = PriceRetail,
                Unit = ProductUnit
            };

            var actual = _mapper.Map<ProductViewModel>(product);

            //Assert.StrictEqual();Equal();

        }
    }
}
