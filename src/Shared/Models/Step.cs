using System;
using System.ComponentModel.DataAnnotations;
using DevOpsLab.Shared.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace DevOpsLab.Shared.Models
{
    public class Step : BaseRankedModel
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            BaseModel.OnModelCreating<Step>(modelBuilder);
        }

        [Required] public Guid ScenarioId { get; set; }
        public virtual Scenario Scenario { get; set; }
    }
}