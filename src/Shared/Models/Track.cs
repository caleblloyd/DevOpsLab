using System.Collections.Generic;
using DevOpsLab.Shared.Models.BaseModels;
using DevOpsLab.Shared.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class Track : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseRankedModel.OnModelCreating<Track>(modelBuilder);
            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasMany(m => m.TrackCourses)
                    .WithOne(m => m.Track)
                    .HasForeignKey(m => m.TrackId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(m => m.TrainingCodeTracks)
                    .WithOne(m => m.Track)
                    .HasForeignKey(m => m.TrackId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public virtual RankedList<TrackCourse> TrackCourses { get; set; } = new RankedList<TrackCourse>();
        
        public virtual RankedList<TrainingCodeTrack> TrainingCodeTracks { get; set; } = new RankedList<TrainingCodeTrack>();
    }
}