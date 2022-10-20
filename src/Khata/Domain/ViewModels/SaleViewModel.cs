using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Domain;

namespace ViewModels;

public class SaleViewModel
{
    public int? Id { get; set; }
    public SaleType Type { get; set; }

    [Display(Name = "Outlet")]
    public int OutletId { get; set; }
        
    public int CustomerId { get; set; }

    [Display(Name = "Add New Customer")]
    public bool RegisterNewCustomer { get; set; }

    [Display(Name = "Buyer")]
    public CustomerViewModel Customer { get; set; }

    [Display(Name = "Cart")]
    public List<LineItemViewModel> Cart { get; set; } = new List<LineItemViewModel>();

    public PaymentInfoViewModel Payment { get; set; } = new PaymentInfoViewModel();

    [Display(Name = "Date")]
    public string SaleDate { get; set; }

    public string Description { get; set; }
}