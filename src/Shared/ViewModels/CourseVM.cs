using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevOpsLab.Shared.ViewModels
{
    public class CourseVM
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Alias { get; set; }

        [Required] public string Description { get; set; }

        public IEnumerable<ScenarioVM> Scenarios { get; set; } = new List<ScenarioVM>();

        public IEnumerable<TrackVM> Tracks { get; set; } = new List<TrackVM>();
    }
}
