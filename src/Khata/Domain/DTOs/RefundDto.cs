using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Domain;

namespace DTOs
{
    public class RefundDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public int CustomerId { get; set; }

        public CustomerDto Customer { get; set; }

        public int SaleId { get; set; }

        public SaleDto Sale { get; set; }

        public ICollection<SaleLineItem> Cart { get; set; }

        [DataType(DataType.Currency)]
        public decimal CashBack { get; set; }

        [DataType(DataType.Currency)]
        public decimal DebtRollback { get; set; }

        public string Description { get; set; }
        public Metadata Metadata { get; set; }
    }
}
