using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class TrainingCodeTrack : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseRankedModel.OnModelCreating<TrainingCodeTrack>(modelBuilder);
            modelBuilder.Entity<TrainingCodeTrack>(entity =>
            {
                entity.HasOne(m => m.TrainingCode)
                    .WithMany(m => m.TrainingCodeTracks)
                    .HasForeignKey(m => m.TrainingCodeId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.Track)
                    .WithMany(m => m.TrainingCodeTracks)
                    .HasForeignKey(m => m.TrackId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        [Required] public Guid TrainingCodeId { get; set; }
        public virtual TrainingCode TrainingCode { get; set; }

        [Required] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }
    }
}