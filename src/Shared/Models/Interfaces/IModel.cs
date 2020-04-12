using System;

namespace DevOpsLab.Shared.Models.Interfaces
{
    public interface IModel
    {
        Guid Id { get; set; }

        DateTime Created { get; set; }
    }
}
