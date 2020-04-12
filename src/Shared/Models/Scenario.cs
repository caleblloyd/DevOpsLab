using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using DevOpsLab.Shared.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
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
            });
        }

        [Required] public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual RankedList<Step> Steps { get; set; } = new RankedList<Step>();
    }
}
