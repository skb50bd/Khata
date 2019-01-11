using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [Display(Name = "Title")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
