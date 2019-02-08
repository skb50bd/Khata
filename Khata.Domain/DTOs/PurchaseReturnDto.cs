using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class PurchaseReturnDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public int SupplierId { get; set; }

        public SupplierDto Supplier { get; set; }

        public int PurchaseId { get; set; }

        public PurchaseDto Purchase { get; set; }

        public ICollection<PurchaseLineItem> Cart { get; set; }

        [DataType(DataType.Currency)]
        public decimal CashBack { get; set; }

        [DataType(DataType.Currency)]
        public decimal DebtRollback { get; set; }

        public string Description { get; set; }
        public Metadata Metadata { get; set; }
    }
}
