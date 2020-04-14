using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
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

        public static implicit operator TrainingCodeAppUserVM(TrainingCodeAppUser model)
        {
            return new TrainingCodeAppUserVM
            {
                Id = model.Id,
                TrainingCode = model.TrainingCode,
                AppUser = model.AppUser,
                Expires = model.Expires
            };
        }

        public Guid TrainingCodeId { get; set; }
        public virtual TrainingCode TrainingCode { get; set; }

        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public bool Active { get; set; } = true;

        public DateTimeOffset? Expires { get; set; }
    }
}
