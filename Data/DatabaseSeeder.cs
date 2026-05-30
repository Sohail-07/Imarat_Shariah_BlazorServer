using Imarat_Shariah.Data.Entities.Identity;
using Imarat_Shariah.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Imarat_Shariah.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Create Admin Role if not exists
            if (!await roleManager.RoleExistsAsync(ApplicationPermissions.Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(ApplicationPermissions.Roles.Admin));
            }

            // 2. Create Master Admin User
            var adminEmail = "admin@imaratshariah.org";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Production me set secure strong password
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, ApplicationPermissions.Roles.Admin);

                    // 3. Assign Master Claims to Admin Role
                    var adminRole = await roleManager.FindByNameAsync(ApplicationPermissions.Roles.Admin);
                    
                    // Utility se direct saari permissions loop me chalao
                    var allPermissions = ApplicationPermissions.GetAllPermissions();

                    foreach (var permission in allPermissions)
                    {
                        // Check lagao taaki duplicate claims add na hon database me
                        var existingClaims = await roleManager.GetClaimsAsync(adminRole!);
                        if (!existingClaims.Any(c => c.Value == permission))
                        {
                            await roleManager.AddClaimAsync(adminRole!, new Claim(ApplicationPermissions.PermissionClaimType, permission));
                        }
                    }
                }
            }
        }
    }
}