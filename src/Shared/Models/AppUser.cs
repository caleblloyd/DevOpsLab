using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
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

        public virtual List<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; } = new List<TrainingCodeAppUser>();
    }
}