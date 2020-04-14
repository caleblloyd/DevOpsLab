using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevOpsLab.Shared.ViewModels
{
    public class ScenarioVM
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        [Required] public string Description { get; set; }

        public CourseVM Course { get; set; }

        public IEnumerable<StepVM> Steps { get; set; } = new List<StepVM>();
    }
}
