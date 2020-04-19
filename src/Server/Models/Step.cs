using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Server.Models.Interfaces;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class Step : BaseRankedModel, IHasViewModel<StepVM>
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<Step>(modelBuilder);
        }

        private StepVM _viewModel;

        [NotMapped]
        public StepVM ViewModel
        {
            get
            {
                return _viewModel ??= new StepVM
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Scenario = Scenario.ViewModel
                };
            }
        }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        public Guid ScenarioId { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}
