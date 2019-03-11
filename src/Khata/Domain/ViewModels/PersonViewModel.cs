using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public abstract class PersonViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MinLength(1,
            ErrorMessage = "First Name must have at least 5 characters.")]
        [MaxLength(35)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MinLength(1,
            ErrorMessage = "Last Name must have at least 5 characters.")]
        [MaxLength(35)]
        public string LastName { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Note { get; set; }
    }
}