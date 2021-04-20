using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSupportApp.Domain.Models;

namespace TechSupportApp.Infrastructure.Identity
{
    public static class SeedIdentity
    {
        public static async Task SeedInitialRoles(RoleManager<IdentityRole> roleManager)
        {
            IList<IdentityRole> roles = new List<IdentityRole>(2);
            bool roleExists = await roleManager.RoleExistsAsync("NormalUser");
            if (!roleExists)
            {
                var role = new IdentityRole
                {
                    Name = "NormalUser"
                };
                roles.Add(role);
            }

            roleExists = await roleManager.RoleExistsAsync("Administrator");
            if (!roleExists)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };

                roles.Add(role);
            }

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
        internal static async Task CreateSuperUser(UserManager<AppIdentityUser> userManager)
        {
            const string UserName = "Super";            

            if ((await userManager.FindByNameAsync(UserName) ) is null)
            {
                var user = new AppIdentityUser
                {
                    UserName = UserName
                };

                await userManager.CreateAsync(user, "Secret1234%");
                await userManager.AddToRoleAsync(user, "Administrator");
            }            

        }
        public static async Task SeedRolesAndCreateSuper(RoleManager<IdentityRole> roleManager, 
            UserManager<AppIdentityUser> userManager)
        {
            await SeedInitialRoles(roleManager);
            await CreateSuperUser(userManager);
        }
    }


}
