using Khata.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Khata.DTOs
{
    public class OutletDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Metadata Metadata { get; set; }

        public ICollection<ProductDto> Products { get; set; }
        public ICollection<SaleDto> Sales { get; set; }
    }
}
