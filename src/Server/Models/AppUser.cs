using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared;
using DevOpsLab.Shared.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class AppUser : IdentityUser, IHasViewModel<AppUserVM>
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasMany(m => m.TrainingCodeAppUsers)
                    .WithOne(m => m.AppUser)
                    .HasForeignKey(m => m.AppUserId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);

                // Navigation Properties Begin
                // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#add-all-user-navigation-properties
                entity.HasMany(e => e.UserClaims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                // Navigation Properties End
            });
        }

        private AppUserVM _viewModel;

        [NotMapped]
        public AppUserVM ViewModel
        {
            get
            {
                return _viewModel ??= new AppUserVM
                {
                    Id = Id,
                    Name = Name,
                    Email = UserName,
                    Role = Role,
                    TrainingCodeAppUsers = TrainingCodeAppUsers.Select(m => m.ViewModel)
                };
            }
        }

        [NotMapped] public string Name => UserClaims.FirstOrDefault(m => m.ClaimType == JwtClaimTypes.Name)?.ClaimValue;

        [NotMapped]
        public string Role
        {
            get
            {
                var roleNames = UserRoles.Select(m => m.Role.Name);
                // ReSharper disable once PossibleMultipleEnumeration
                if (roleNames.Contains(RoleTypes.Admin))
                {
                    return RoleTypes.Admin;
                }

                // ReSharper disable once PossibleMultipleEnumeration
                if (roleNames.Contains(RoleTypes.Instructor))
                {
                    return RoleTypes.Instructor;
                }

                return "Student";
            }
        }

        public virtual List<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; } =
            new List<TrainingCodeAppUser>();

        // Navigation Properties Begin
        // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#add-user-and-role-navigation-properties
        public virtual ICollection<IdentityUserClaim<string>> UserClaims { get; set; }

        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        // Navigation Properties End
    }
}
