﻿using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;

namespace WebUI.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly IEmployeeService _employees;
        private readonly PfService _pfService;
        public IndexModel(IEmployeeService employees, PfService pfService)
        {
            _employees = employees;
            _pfService = pfService;
            Employees = new PagedList<EmployeeDto>();
        }

        public IPagedList<EmployeeDto> Employees { get; set; }
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
            Employees = await _employees.Get(Pf);
            return Page();
        }
    }
}
