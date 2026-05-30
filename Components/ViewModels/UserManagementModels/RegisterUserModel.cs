namespace Imarat_Shariah.Components.ViewModels.UserManagementModels
{
    public class RegisterUserModel
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
