using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters long")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Outlet")]
        public int OutletId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}