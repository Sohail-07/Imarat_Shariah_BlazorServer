using Imarat_Shariah.Components.ViewModels.UserManagementModels;
using Imarat_Shariah.Data.Entities.Identity;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Imarat_Shariah.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public UserProfileService(UserManager<ApplicationUser> userManager, AuthenticationStateProvider authStateProvider)
        {
            _userManager = userManager;
            _authStateProvider = authStateProvider;
        }

        // Current identity user context verify karne ke liye helper method
        private async Task<ApplicationUser> GetCurrentApplicationUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = await _userManager.GetUserAsync(authState.User);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Session expire ho chuki hai, kripya fir se login karein.");
            }

            return user;
        }

        // 1. Profile Data Fetch Logic
        public async Task<MyProfileViewModel> GetCurrentProfileAsync()
        {
            var user = await GetCurrentApplicationUserAsync();

            return new MyProfileViewModel
            {
                FullName = user?.FullName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                MobileNumber = user?.PhoneNumber ?? string.Empty,
                Address = user?.Address ?? string.Empty
            };
        }

        // 2. Profile Data Update Logic
        public async Task UpdateProfileAsync(MyProfileViewModel model)
        {
            var user = await GetCurrentApplicationUserAsync();

            user.FullName = model.FullName;
            user.PhoneNumber = model.MobileNumber; // Identity framework base property mapping
            user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Profile update failed.";
                throw new Exception(error);
            }
        }

        // 3. Password Management Logic
        public async Task<(bool IsSuccess, string? Error)> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            var user = await GetCurrentApplicationUserAsync();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                // Password change hone ke baad security stamp update karna standard rule h
                await _userManager.UpdateSecurityStampAsync(user);
                return (true, null);
            }

            return (false, result.Errors.FirstOrDefault()?.Description ?? "Password change fails.");
        }
    }
}
