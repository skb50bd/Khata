namespace Domain
{
    public class Pricing
    {
        public decimal Purchase { get; set; }
        public decimal Retail { get; set; }
        public decimal Bulk { get; set; }
        public decimal Margin { get; set; }

        public decimal ProfitIfSellingPriceIs(decimal sellingPrice) => sellingPrice - Purchase;
        public bool IsPriceAllowed(decimal sellingPrice) => sellingPrice >= Margin;
    }
}