using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class AppRole : IdentityRole
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>(entity =>
            {
                // Navigation Properties Begin
                // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#add-all-user-navigation-properties
                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                // Navigation Properties End
            });
        }

        // Navigation Properties Begin
        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#add-all-user-navigation-properties
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        // Navigation Properties End
    }
}
