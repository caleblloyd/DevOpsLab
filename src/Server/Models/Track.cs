using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
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
                entity.HasIndex(m => m.Alias);
            });
        }

        private TrackVM ViewModel { get; set; }

        public static implicit operator TrackVM(Track model)
        {
            return model.ViewModel ??= new TrackVM
            {
                Id = model.Id,
                Name = model.Name,
                Alias = model.Alias,
                Courses = model.TrackCourses
                    .Select<TrackCourse, CourseVM>(m => m.Course),
                TrainingCodes = model.TrainingCodeTracks
                    .Select<TrainingCodeTrack, TrainingCodeVM>(m => m.TrainingCode)
            };
        }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        public virtual RankedList<TrackCourse> TrackCourses { get; set; } = new RankedList<TrackCourse>();

        public virtual RankedList<TrainingCodeTrack> TrainingCodeTracks { get; set; } =
            new RankedList<TrainingCodeTrack>();
    }
}
