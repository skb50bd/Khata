using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModels
{
    public class OutletViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Outlet Name")]
        public string Title { get; set; }

        public string Slogan { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
