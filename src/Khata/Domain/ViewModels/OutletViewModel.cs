using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class OutletViewModel
{
    public int Id { get; set; }

    [Display(Name = "Outlet Name")]
    public string Title { get; set; }

    public string Slogan { get; set; }

    public string Address { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}