using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class PurchaseReturnViewModel
{
    public int? Id { get; set; }

    public int PurchaseId { get; set; }

    public ICollection<LineItemViewModel> Cart { get; set; }


    [DataType(DataType.Currency)]
    [Display(Name = "Cash Back")]
    public decimal CashBack { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Debt Rollback")]
    public decimal DebtRollback { get; set; }

    public string Description { get; set; }
}