﻿using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Incoming.Pages.PurchaseReturns
{
    public class IndexModel : PageModel
    {
        private readonly IPurchaseReturnService _purchaseReturns;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, IPurchaseReturnService purchaseReturns)
        {
            _pfService = pfService;
            _purchaseReturns = purchaseReturns;
            PurchaseReturns = new PagedList<PurchaseReturnDto>();
        }

        public IPagedList<PurchaseReturnDto> PurchaseReturns { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            PurchaseReturns = await _purchaseReturns.Get(Pf);
            return Page();
        }
    }
}
