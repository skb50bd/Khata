using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public Metadata Metadata { get; set; }
    }
}