using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class SupplierViewModel : PersonViewModel
    {
        [DataType(DataType.Currency)]
        public decimal Payable { get; set; }
    }
}