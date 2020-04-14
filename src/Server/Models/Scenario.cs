using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Collections;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class Scenario : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<Scenario>(modelBuilder);
            modelBuilder.Entity<Scenario>(entity =>
            {
                entity.HasMany(m => m.Steps)
                    .WithOne(m => m.Scenario)
                    .HasForeignKey(m => m.ScenarioId)
                    .HasPrincipalKey(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(m => m.Alias);
            });
        }

        public static implicit operator ScenarioVM(Scenario model)
        {
            return new ScenarioVM
            {
                Id = model.Id,
                Name = model.Name,
                Alias = model.Alias,
                Description = model.Description,
                Course = model.Course,
                Steps = model.Steps
                    .Select<Step, StepVM>(m => m)
            };
        }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        [Required] public string Description { get; set; }

        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual RankedList<Step> Steps { get; set; } =
            new RankedList<Step>();
    }
}
