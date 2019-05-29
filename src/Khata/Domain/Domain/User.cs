using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName 
            => FirstName + " " + LastName;

        public Role Role { get; set; }

        private string _avatar;

        public string Avatar
        {
            get => _avatar ?? "user.png";
            set => _avatar = value;
        }
    }
}