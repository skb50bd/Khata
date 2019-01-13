using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class CustomerViewModel : PersonViewModel
    {
        [DataType(DataType.Currency)]
        public decimal Debt { get; set; } = 0M;
    }
}