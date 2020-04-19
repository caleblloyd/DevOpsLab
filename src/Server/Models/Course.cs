using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class Course : BaseModel
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

        private CourseVM ViewModel { get; set; }

        public static implicit operator CourseVM(Course model)
        {
            return model.ViewModel ??= new CourseVM
            {
                Id = model.Id,
                Name = model.Name,
                Alias = model.Alias,
                Description = model.Description,
                Scenarios = model.Scenarios
                    .Select<Scenario, ScenarioVM>(m => m),
                Tracks = model.TrackCourses
                    .Select<TrackCourse, TrackVM>(m => m.Track)
            };
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
