using System.ComponentModel.DataAnnotations;

namespace Khata.DTOs
{
    public class EmployeeDto : PersonDto
    {
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }

        [Display(Name = "NID Number")]
        public string NIdNumber { get; set; }
    }
}
