using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Products.Components.ProductsList
{
    public class ProductsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ProductDto> products)
        {
            return View(nameof(ProductsList), products);
        }
    }
}
