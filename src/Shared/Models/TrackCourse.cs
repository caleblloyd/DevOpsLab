using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class TrackCourse : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseRankedModel.OnModelCreating<TrackCourse>(modelBuilder);
            modelBuilder.Entity<TrackCourse>(entity =>
            {
                entity.HasOne(m => m.Track)
                    .WithMany(m => m.TrackCourses)
                    .HasForeignKey(m => m.TrackId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.Course)
                    .WithMany(m => m.TrackCourses)
                    .HasForeignKey(m => m.CourseId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        [Required] public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        [Required] public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}