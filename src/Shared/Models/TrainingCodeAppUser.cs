using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class TrainingCodeAppUser : BaseModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<TrainingCodeAppUser>(modelBuilder);
            modelBuilder.Entity<TrainingCodeAppUser>(entity =>
            {
                entity.HasOne(m => m.TrainingCode)
                    .WithMany(m => m.TrainingCodeAppUsers)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.AppUser)
                    .WithMany(m => m.TrainingCodeAppUsers)
                    .HasForeignKey(m => m.AppUserId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        [Required] public Guid TrainingCodeId { get; set; }
        public virtual TrainingCode TrainingCode { get; set; }

        [Required] public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
