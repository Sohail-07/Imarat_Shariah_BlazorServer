using Imarat_Shariah.Components.ViewModels.UserManagementModels;
using Imarat_Shariah.Data.Entities.Identity;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface IInternalUserManager
    {
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<(bool IsSuccess, string? Error)> CreateStaffUserAsync(RegisterUserModel model);
        Task<List<PermissionSelection>> GetUserPermissionsAsync(string userId);
        Task UpdateUserPermissionsAsync(string userId, List<PermissionSelection> permissions);
        Task ToggleUserLockoutAsync(string userId);
        Task<(bool IsSuccess, string? Error)> ForceResetPasswordAsync(string userId, string newPassword);
    }
}
