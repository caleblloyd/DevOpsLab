using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevOpsLab.Shared.ViewModels
{
    public class TrainingCodeVM
    {
        public Guid Id { get; set; }

        [Required] public string Code { get; set; }

        public int MaxUsers { get; set; }

        public TimeSpan? ExpiresAfter { get; set; }

        public IEnumerable<TrainingCodeAppUserVM> TrainingCodeAppUsers { get; set; } =
            new List<TrainingCodeAppUserVM>();

        public IEnumerable<TrackVM> Tracks { get; set; } =
            new List<TrackVM>();
    }
}
