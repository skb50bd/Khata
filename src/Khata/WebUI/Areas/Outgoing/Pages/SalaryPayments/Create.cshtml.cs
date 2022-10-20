using System.Threading.Tasks;

using Business.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ViewModels;

namespace WebUI.Areas.Outgoing.Pages.SalaryPayments;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ISalaryPaymentService _salaryPayments;

    public CreateModel(ISalaryPaymentService salaryPayments)
    {
        _salaryPayments = salaryPayments;
    }

    public IActionResult OnGet()
    {
        SalaryPaymentVm = new SalaryPaymentViewModel();
        return Page();
    }

    [BindProperty]
    public SalaryPaymentViewModel SalaryPaymentVm { get; set; }

    [TempData] public string Message { get; set; }
    [TempData] public string MessageType { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var salaryPayment = await _salaryPayments.Add(SalaryPaymentVm);

        Message = $"Debt-Payment: {salaryPayment.Id} - {salaryPayment.EmployeeFullName} - {salaryPayment.Amount} created!";
        MessageType = "success";


        return RedirectToPage("./Index");
    }
}