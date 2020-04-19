using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class Track : BaseRankedModel, IHasViewModel<TrackVM>
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

        private TrackVM _viewModel;

        [NotMapped]
        public TrackVM ViewModel
        {
            get
            {
                return _viewModel ??= new TrackVM
                {
                    Id = Id,
                    Name = Name,
                    Alias = Alias,
                    Courses = TrackCourses.Select(m => m.Course.ViewModel),
                    TrainingCodes = TrainingCodeTracks.Select(m => m.TrainingCode.ViewModel)
                };
            }
        }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        public virtual RankedList<TrackCourse> TrackCourses { get; set; } = new RankedList<TrackCourse>();

        public virtual RankedList<TrainingCodeTrack> TrainingCodeTracks { get; set; } =
            new RankedList<TrainingCodeTrack>();
    }
}
