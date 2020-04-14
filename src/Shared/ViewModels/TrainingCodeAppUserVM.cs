using System;

namespace DevOpsLab.Shared.ViewModels
{
    public class TrainingCodeAppUserVM
    {
        public Guid Id { get; set; }

        public TrainingCodeVM TrainingCode { get; set; }

        public virtual AppUserVM AppUser { get; set; }

        public DateTimeOffset? Expires { get; set; }
    }
}
