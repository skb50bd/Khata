using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Products.Components.ProductInfo
{
    public class ProductInfo : ViewComponent
    {
        public IViewComponentResult Invoke(ProductDto product)
        {
            return View(nameof(ProductInfo), product);
        }

    }
}
