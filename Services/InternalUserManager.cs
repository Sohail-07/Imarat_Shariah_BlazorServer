using Imarat_Shariah.Components.ViewModels.UserManagementModels;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Services
{
    public class InternalUserManager : IInternalUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public InternalUserManager(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync() =>
            await _userManager.Users.ToListAsync();

        public async Task<(bool IsSuccess, string? Error)> CreateStaffUserAsync(RegisterUserModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Default staff role map kar do agar chahiye to
                return (true, null);
            }
            return (false, result.Errors.FirstOrDefault()?.Description ?? "Registration Failed");
        }

        public async Task<List<PermissionSelection>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new();

            // Check user existing claims from database
            var userClaims = await _userManager.GetClaimsAsync(user);
            var existingPermissions = userClaims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();

            // All system permissions blueprint
            var allPermissions = new List<PermissionSelection>
            {
                new() { DisplayName = "Siyajat - View Records", PermissionValue = "Permissions.Siyajat.View" },
                new() { DisplayName = "Siyajat - Create New", PermissionValue = "Permissions.Siyajat.Create" },
                new() { DisplayName = "Siyajat - Update/Edit", PermissionValue = "Permissions.Siyajat.Update" },
                new() { DisplayName = "Siyajat - Delete Record", PermissionValue = "Permissions.Siyajat.Delete" },
                new() { DisplayName = "Khula - View Records", PermissionValue = "Permissions.Khula.View" },
                new() { DisplayName = "Khula - Create New", PermissionValue = "Permissions.Khula.Create" },
                new() { DisplayName = "Khula - Update/Edit", PermissionValue = "Permissions.Khula.Update" },
                new() { DisplayName = "Khula - Delete Record", PermissionValue = "Permissions.Khula.Delete" },
            };

            foreach (var p in allPermissions)
            {
                if (existingPermissions.Contains(p.PermissionValue))
                    p.IsSelected = true;
            }

            return allPermissions;
        }

        public async Task UpdateUserPermissionsAsync(string userId, List<PermissionSelection> permissions)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            // Clean slate: Purane saare permission claims delete karo user ke
            var currentClaims = await _userManager.GetClaimsAsync(user);
            var permissionClaims = currentClaims.Where(c => c.Type == "Permission");
            foreach (var claim in permissionClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Selected checkboxed permissions inject karo dobara
            foreach (var p in permissions.Where(x => x.IsSelected))
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Permission", p.PermissionValue));
            }

            // Security stamp refresh karo taaki persistent circuit immediate update pull kare
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }
}
