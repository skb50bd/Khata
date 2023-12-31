﻿using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outgoing.Pages.SalaryPayments
{
    public class IndexModel : PageModel
    {
        private readonly ISalaryPaymentService _salaryPayments;
        private readonly PfService _pfService;
        public IndexModel(ISalaryPaymentService salaryPayments, PfService pfService)
        {
            _salaryPayments = salaryPayments;
            _pfService = pfService;
            SalaryPayments = new PagedList<SalaryPaymentDto>();
        }

        public IPagedList<SalaryPaymentDto> SalaryPayments { get; set; }
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
            SalaryPayments = await _salaryPayments.Get(Pf);
            return Page();
        }
    }
}
