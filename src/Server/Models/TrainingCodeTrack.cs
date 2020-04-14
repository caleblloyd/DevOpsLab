using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Server.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
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

        public Guid TrainingCodeId { get; set; }
        public virtual TrainingCode TrainingCode { get; set; }

        public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }
    }
}
