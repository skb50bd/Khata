using System;

namespace Khata.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset ModificationTime { get; set; }
        public string Modifier { get; set; }
    }
}