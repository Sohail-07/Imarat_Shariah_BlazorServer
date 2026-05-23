using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Imarat_Shariah.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Create Admin Role if not exists
            const string adminRoleName = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // 2. Create Master Admin User
            var adminEmail = "admin@imaratshariah.org";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Production me set secure strong password
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);

                    // 3. Assign Master Claims to Admin Role
                    var adminRole = await roleManager.FindByNameAsync(adminRoleName);
                    var allPermissions = new List<string>
                    {
                        "Permissions.Siyajat.View", "Permissions.Siyajat.Create", "Permissions.Siyajat.Update", "Permissions.Siyajat.Delete",
                        "Permissions.Khula.View", "Permissions.Khula.Create", "Permissions.Khula.Update", "Permissions.Khula.Delete"
                    };

                    foreach (var permission in allPermissions)
                    {
                        await roleManager.AddClaimAsync(adminRole!, new Claim("Permission", permission));
                    }
                }
            }
        }
    }
}