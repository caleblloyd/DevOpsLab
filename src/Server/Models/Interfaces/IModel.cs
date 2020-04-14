using System;

namespace DevOpsLab.Server.Models.Interfaces
{
    public interface IModel
    {
        Guid Id { get; set; }

        DateTime Created { get; set; }
    }
}
