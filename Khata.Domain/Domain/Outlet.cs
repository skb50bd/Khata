using System;
using System.Collections.Generic;
using System.Text;

namespace Khata.Domain
{
    public class Outlet : TrackedDocument
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
