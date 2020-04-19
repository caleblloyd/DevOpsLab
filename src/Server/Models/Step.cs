using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Server.Models.BaseModels;
using DevOpsLab.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Server.Models
{
    public class Step : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<Step>(modelBuilder);
        }
        
        private StepVM ViewModel { get; set; }

        public static implicit operator StepVM(Step model)
        {
            return model.ViewModel ??= new StepVM
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Scenario = model.Scenario
            };
        }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        public Guid ScenarioId { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}
