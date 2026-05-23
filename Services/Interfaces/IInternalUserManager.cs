using Imarat_Shariah.Components.ViewModels.UserManagementModels;
using Microsoft.AspNetCore.Identity;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface IInternalUserManager
    {
        Task<List<IdentityUser>> GetAllUsersAsync();
        Task<(bool IsSuccess, string? Error)> CreateStaffUserAsync(RegisterUserModel model);
        Task<List<PermissionSelection>> GetUserPermissionsAsync(string userId);
        Task UpdateUserPermissionsAsync(string userId, List<PermissionSelection> permissions);
    }
}
