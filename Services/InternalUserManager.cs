using Imarat_Shariah.Components.ViewModels.UserManagementModels;
using Imarat_Shariah.Data.Entities.Identity;
using Imarat_Shariah.Services.Interfaces;
using Imarat_Shariah.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Services
{
    public class InternalUserManager : IInternalUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public InternalUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync() =>
            await _userManager.Users.ToListAsync();

        public async Task<(bool IsSuccess, string? Error)> CreateStaffUserAsync(RegisterUserModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FullName = model.FullName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber
            };
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
            var existingPermissions = userClaims.Where(c => c.Type == ApplicationPermissions.PermissionClaimType).Select(c => c.Value).ToList();

            // All system permissions blueprint
            // Mapping using Centralized Constants Class
            var allPermissions = new List<PermissionSelection>
            {
                new() { DisplayName = "Siyajat - View Records", PermissionValue = ApplicationPermissions.Siyajat.View },
                new() { DisplayName = "Siyajat - Create New", PermissionValue = ApplicationPermissions.Siyajat.Create },
                new() { DisplayName = "Siyajat - Update/Edit", PermissionValue = ApplicationPermissions.Siyajat.Update },
                new() { DisplayName = "Siyajat - Delete Record", PermissionValue = ApplicationPermissions.Siyajat.Delete },

                new() { DisplayName = "Khula - View Records", PermissionValue = ApplicationPermissions.Khula.View },
                new() { DisplayName = "Khula - Create New", PermissionValue = ApplicationPermissions.Khula.Create },
                new() { DisplayName = "Khula - Update/Edit", PermissionValue = ApplicationPermissions.Khula.Update },
                new() { DisplayName = "Khula - Delete Record", PermissionValue = ApplicationPermissions.Khula.Delete }
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
            var permissionClaims = currentClaims.Where(c => c.Type == ApplicationPermissions.PermissionClaimType);
            foreach (var claim in permissionClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Selected checkboxed permissions inject karo dobara
            foreach (var p in permissions.Where(x => x.IsSelected))
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ApplicationPermissions.PermissionClaimType, p.PermissionValue));
            }

            // Security stamp refresh karo taaki persistent circuit immediate update pull kare
            await _userManager.UpdateSecurityStampAsync(user);
        }

        // Toggle Lockout / Account Block Logic
        public async Task ToggleUserLockoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                // Unblock: Lockout time khatam kar do
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                // Block: Agle 100 saal tak ke liye lock kar do account
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
            await _userManager.UpdateSecurityStampAsync(user); // Force clear active browser sessions
        }

        // Force Reset Password by Admin
        public async Task<(bool IsSuccess, string? Error)> ForceResetPasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return (false, "User nahi mila.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return (true, null);
            }
            return (false, result.Errors.FirstOrDefault()?.Description);
        }
    }
}