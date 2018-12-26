using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MinLength(5,
            ErrorMessage = "Last Name must have at least 5 characters.")]
        [MaxLength(35)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(5,
            ErrorMessage = "Last Name must have at least 5 characters.")]
        [MaxLength(35)]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [Display(Name = "Company Name")]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal Debt => -1M * Balance;
    }
}