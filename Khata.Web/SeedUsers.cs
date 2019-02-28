using System.Threading.Tasks;

using Khata.Domain;

using Microsoft.AspNetCore.Identity;

using static Khata.Domain.Role;

namespace WebUI
{
    public static class SeedUsers
    {
        public static async Task Seed(
            RoleManager<IdentityRole> _roleManager, 
            UserManager<ApplicationUser> _userManager)
        {
            var user = new ApplicationUser();
            bool x = await _roleManager.RoleExistsAsync(Admin.ToString());
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = Admin.ToString();
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website
                user = new ApplicationUser();
                user.UserName = "brotal";
                user.Email = "skb50bd@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Pwd+123";

                IdentityResult chkUser = 
                    await _userManager.CreateAsync(
                        user, 
                        userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = 
                        await _userManager.AddToRoleAsync(
                            user, 
                            Admin.ToString());
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync(Manager.ToString());
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = Manager.ToString();
                await _roleManager.CreateAsync(role);
                var result3 = 
                    await _userManager.AddToRoleAsync(
                        user, 
                        Manager.ToString());
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync(
                Role.Employee.ToString());
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = Role.Employee.ToString();
                await _roleManager.CreateAsync(role);
                var result5 = 
                    await _userManager.AddToRoleAsync(
                        user, 
                        Role.Employee.ToString());
            }

            // creating Creating User role     
            x = await _roleManager.RoleExistsAsync(User.ToString());
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = User.ToString();
                await _roleManager.CreateAsync(role);
                var result2 = 
                    await _userManager.AddToRoleAsync(
                        user, 
                        User.ToString());
            }
        }
    }
}
