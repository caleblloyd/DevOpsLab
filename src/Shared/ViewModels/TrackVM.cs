using System;
using System.Collections.Generic;

namespace DevOpsLab.Shared.ViewModels
{
    public class TrackVM
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Alias { get; set; }

        public IEnumerable<CourseVM> Courses { get; set; } = new List<CourseVM>();

        public IEnumerable<TrainingCodeVM> TrainingCodes { get; set; } = new List<TrainingCodeVM>();
    }
}
