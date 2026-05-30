using System.ComponentModel.DataAnnotations;

namespace Imarat_Shariah.Components.ViewModels.UserManagementModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Purana password dalna zaroori hai.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naya password zaroori hai.")]
        [MinLength(6, ErrorMessage = "Password kam se kam 6 characters ka hona chahiye.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password confirm karein.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Naya password aur confirm password match nahi ho rahe hain.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
