using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        public bool IsRemoved { get; set; }


        [Display(Name = "Title")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public Metadata Metadata { get; set; }
    }
}
