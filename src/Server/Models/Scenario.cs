using System;
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
    public class Scenario : BaseRankedModel, IHasViewModel<ScenarioVM>
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

        private ScenarioVM _viewModel;

        [NotMapped]
        public ScenarioVM ViewModel
        {
            get
            {
                return _viewModel ??= new ScenarioVM
                {
                    Id = Id,
                    Name = Name,
                    Alias = Alias,
                    Description = Description,
                    Course = Course.ViewModel,
                    Steps = Steps.Select(m => m.ViewModel)
                };
            }
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
