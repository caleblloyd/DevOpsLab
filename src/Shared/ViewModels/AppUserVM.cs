using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevOpsLab.Shared.ViewModels
{
    public class AppUserVM
    {
        public string Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }

        [Required] public string Role { get; set; }

        public IEnumerable<TrainingCodeAppUserVM> TrainingCodeAppUsers { get; set; } =
            new List<TrainingCodeAppUserVM>();
    }
}
