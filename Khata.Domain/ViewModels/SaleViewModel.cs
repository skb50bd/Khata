using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public SaleType Type { get; set; }
        public int CustomerId { get; set; }

        public ICollection<SaleLineItem> Cart { get; set; }
        public PaymentInfo Payment { get; set; }

        [Display(Name = "Date")]
        public string SaleDate { get; set; }

        public decimal Description { get; set; }

    }
}