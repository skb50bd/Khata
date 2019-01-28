using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public abstract class PersonDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Note { get; set; }

        public Metadata Metadata { get; set; }
    }
}