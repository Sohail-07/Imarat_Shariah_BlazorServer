using Microsoft.AspNetCore.Identity;

namespace Imarat_Shariah.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; }

        [PersonalData]
        public string? Address { get; set; }
    }
}