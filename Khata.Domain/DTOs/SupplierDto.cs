using System.ComponentModel.DataAnnotations;

namespace Khata.DTOs
{
    public class SupplierDto : PersonDto
    {
        [DataType(DataType.Currency)]
        public decimal Payable { get; set; }
    }
}
