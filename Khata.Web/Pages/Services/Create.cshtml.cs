﻿using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Services
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public CreateModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            ServiceVM = new ServiceViewModel();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ServiceViewModel ServiceVM { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var service = _mapper.Map<Service>(ServiceVM);
            service.Metadata = Metadata.CreatedNew(User.Identity.Name);
            _db.Services.Add(service);
            await _db.CompleteAsync();

            Message = $"Service: {service.Id} - {service.Name} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}