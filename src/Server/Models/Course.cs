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
    public class Course : BaseModel, IHasViewModel<CourseVM>
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<Course>(modelBuilder);
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasMany(m => m.Scenarios)
                    .WithOne(m => m.Course)
                    .HasForeignKey(m => m.CourseId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(m => m.TrackCourses)
                    .WithOne(m => m.Course)
                    .HasForeignKey(m => m.CourseId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(m => m.Alias);
            });
        }

        private CourseVM _viewModel;

        [NotMapped]
        public CourseVM ViewModel
        {
            get
            {
                return _viewModel ??= new CourseVM
                {
                    Id = Id,
                    Name = Name,
                    Alias = Alias,
                    Description = Description,
                    Scenarios = Scenarios.Select(m => m.ViewModel),
                    Tracks = TrackCourses.Select(m => m.Track.ViewModel)
                };
            }
        }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        [Required] public string Description { get; set; }

        public virtual RankedList<Scenario> Scenarios { get; set; } =
            new RankedList<Scenario>();

        public virtual RankedList<TrackCourse> TrackCourses { get; set; } =
            new RankedList<TrackCourse>();
    }
}
