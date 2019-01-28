using System;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class DebtPaymentDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        public int CustomerId { get; set; }


        [Display(Name = "Customer Name", ShortName = "Payer")]
        public string CustomerFullName { get; set; }

        [Display(Name = "Previous Due")]
        [DataType(DataType.Currency)]
        public decimal DebtBefore { get; set; }

        [Display(Name = "Amount Paid", ShortName = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "New Due")]
        [DataType(DataType.Currency)]
        public decimal DebtAfter { get; set; }

        public string Description { get; set; }

        public Metadata Metadata { get; set; }
    }
}