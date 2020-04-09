using DevOpsLab.Shared.Models.BaseModels;
using DevOpsLab.Shared.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
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
            });
        }
        
        public virtual RankedList<Scenario> Scenarios { get; set; } = new RankedList<Scenario>();

        public virtual RankedList<TrackCourse> TrackCourses { get; set; } = new RankedList<TrackCourse>();
    }
}