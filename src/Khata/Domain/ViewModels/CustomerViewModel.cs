using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class CustomerViewModel : PersonViewModel
    {
        [Display(Name = "Company")]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [DataType(DataType.Currency)]
        public decimal Debt { get; set; } = 0M;
    }
}