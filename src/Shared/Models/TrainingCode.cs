using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using DevOpsLab.Shared.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class TrainingCode : BaseModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<TrainingCode>(modelBuilder);
            modelBuilder.Entity<TrainingCode>(entity =>
            {
                entity.HasMany(m => m.TrainingCodeAppUsers)
                    .WithOne(m => m.TrainingCode)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(m => m.TrainingCodeTracks)
                    .WithOne(m => m.TrainingCode)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(m => m.Code);
            });
        }

        [Required] public string Code { get; set; }

        public int MaxUsers { get; set; }

        public virtual List<TrainingCodeAppUser> TrainingCodeAppUsers { get; set; } = new List<TrainingCodeAppUser>();

        public virtual RankedList<TrainingCodeTrack> TrainingCodeTracks { get; set; } =
            new RankedList<TrainingCodeTrack>();
    }
}
