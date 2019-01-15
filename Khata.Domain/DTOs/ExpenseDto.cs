using System;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Modified by")]
        public string MetadataModifier { get; set; }

        [Display(Name = "Modified at")]
        public DateTimeOffset MetadataModificationTime { get; set; }
    }
}
