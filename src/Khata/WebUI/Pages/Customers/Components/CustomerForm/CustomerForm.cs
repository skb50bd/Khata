using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Customers.Components.CustomerForm
{
    public class CustomerForm : ViewComponent
    {
        public IViewComponentResult Invoke(CustomerViewModel customer)
        {
            return View(nameof(CustomerForm), customer);
        }
    }
}
