using Microsoft.AspNetCore.Identity;

namespace DevOpsLab.Server.Models
{
    public class AppUserRole : IdentityUserRole<string>
    {
        // Navigation Properties Begin
        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#add-all-user-navigation-properties
        public virtual AppUser User { get; set; }
        
        public virtual AppRole Role { get; set; }
        // Navigation Properties End
    }
}
