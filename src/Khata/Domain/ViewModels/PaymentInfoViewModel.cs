using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class PaymentInfoViewModel
    {
        [Display(Name = "Discount Amount")]
        [Range(0, double.MaxValue)]
        public decimal DiscountCash { get; set; }

        [Display(Name = "Discount Percentage", ShortName = "Discount %")]
        [Range(0, 70)]
        public float DiscountPercentage { get; set; }

        public decimal Paid { get; set; }
    }
}
