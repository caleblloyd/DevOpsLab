using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class AppUser : IdentityUser
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
            });
        }

        public static implicit operator AppUserVM(AppUser model)
        {
            return new AppUserVM
            {
                TrainingCodeAppUsers = model.TrainingCodeAppUsers
                    .Select<TrainingCodeAppUser, TrainingCodeAppUserVM>(m => m)
            };
        }
        
        public virtual List<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; } =
            new List<TrainingCodeAppUser>();
    }
}
