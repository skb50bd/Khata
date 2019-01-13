using System.ComponentModel.DataAnnotations;

namespace Khata.DTOs
{
    public class CustomerDto : PersonDto
    {
        [Display(Name = "Due")]
        [DataType(DataType.Currency)]
        public decimal Debt { get; set; }
    }
}