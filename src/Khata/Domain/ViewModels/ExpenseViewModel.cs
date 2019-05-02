using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(3, ErrorMessage = "Title must be of at least 3 characters")]
        [MaxLength(30)]
        [Display(Name = "Expense Title")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
