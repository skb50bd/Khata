using Khata.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Products.Components.ProductForm
{
    public class ProductForm : ViewComponent
    {
        public IViewComponentResult Invoke(ProductViewModel product)
        {
            return View("Default", product);
        }
    }
}
