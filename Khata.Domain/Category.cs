namespace Khata.Domain
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}