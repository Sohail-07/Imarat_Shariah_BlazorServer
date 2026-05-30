using System.ComponentModel.DataAnnotations;

namespace Imarat_Shariah.Components.ViewModels.UserManagementModels
{
    public class MyProfileViewModel
    {
        [Required(ErrorMessage = "Full Name dalein.")]
        [StringLength(100, ErrorMessage = "Name bohot lamba hai.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email dalein.")]
        [EmailAddress(ErrorMessage = "Email ka format sahi nahi hai.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number zaroori hai.")]
        [Phone(ErrorMessage = "Mobile number ka format sahi nahi hai.")]
        [MaxLength(10, ErrorMessage = "Mobile number galat hai.")]
        public string MobileNumber { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;
    }
}
