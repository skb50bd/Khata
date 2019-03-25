using System.Globalization;

namespace Domain
{
    public class OutletOptions
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int DefaultPageSize { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
