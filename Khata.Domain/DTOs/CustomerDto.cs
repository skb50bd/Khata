namespace Khata.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal Debt => -1M * Balance;
        public string MetadataModifier { get; set; }
        public string MetadataModificationTime { get; set; }
    }
}