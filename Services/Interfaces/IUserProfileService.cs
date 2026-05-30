using Imarat_Shariah.Components.ViewModels.UserManagementModels;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<MyProfileViewModel> GetCurrentProfileAsync();
        Task UpdateProfileAsync(MyProfileViewModel model);
        Task<(bool IsSuccess, string? Error)> ChangePasswordAsync(ChangePasswordViewModel model);
    }
}
