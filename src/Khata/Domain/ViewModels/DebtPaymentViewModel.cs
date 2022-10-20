using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class DebtPaymentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public string PaymentDate { get; set; }

    public int CustomerId { get; set; }
    public int InvoiceId { get; set; }

    [Display(Name = "Amount Paid", ShortName = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }
}