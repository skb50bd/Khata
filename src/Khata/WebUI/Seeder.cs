using System.IO;
using System.Threading.Tasks;
using Business.Abstractions;
using Domain;

using Microsoft.AspNetCore.Identity;

using static Domain.Role;

namespace WebUI
{
    public static class Seeder
    {
        public static async Task SeedUsers(
            RoleManager<IdentityRole> roleManager, 
            UserManager<User> userManager)
        {
            var user = new User();
            var x = await roleManager.RoleExistsAsync(Admin.ToString());
            if (!x)
            {
                var role = new IdentityRole {Name = Admin.ToString()};
                await roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website
                user = new User
                {
                    FirstName      = "Admin",
                    LastName       = "Brotal",
                    UserName       = "brotal",
                    Email          = "skb50bd@gmail.com",
                    EmailConfirmed = true
                };

                const string userPwd = "Pwd+123";

                var chkUser = 
                    await userManager.CreateAsync(
                        user, 
                        userPwd);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        user, 
                        Admin.ToString());
                }
            }

            // creating Creating Manager role     
            x = await roleManager.RoleExistsAsync(Manager.ToString());
            if (!x)
            {
                var role = new IdentityRole {Name = Manager.ToString()};
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(
                    user, 
                    Manager.ToString());
            }

            // creating Creating Employee role     
            x = await roleManager.RoleExistsAsync(
                Role.Employee.ToString());
            if (!x)
            {
                var role = new IdentityRole {Name = Role.Employee.ToString()};
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(
                    user, 
                    Role.Employee.ToString());
            }

            // creating Creating User role     
            x = await roleManager.RoleExistsAsync(Role.User.ToString());
            if (!x)
            {
                var role = new IdentityRole {Name = Role.User.ToString()};
                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(
                    user, 
                    Role.User.ToString());
            }
        }

        public static void SeedImages(IFileService store)
        {
            using (var stream = GetFileAsStream(@"Images/user.png"))
            {
                store.Save(
                    "user.png", 
                    stream
                );
            }
        }

        private static Stream GetFileAsStream(string path)
        {
            var ms = new MemoryStream();
            using (var sr = File.OpenRead(path))
            {
                sr.Position = 0;
                sr.CopyTo(ms);
            }

            ms.Position = 0;
            return ms;
        }
    }
}
