namespace Khata.Domain
{
    public class CashRegister : Entity
    {
        public decimal Balance { get; set; } = 0;
        public Metadata Metadata { get; set; }
    }
}
