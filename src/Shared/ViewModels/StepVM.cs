using System;

namespace DevOpsLab.Shared.ViewModels
{
    public class StepVM
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public ScenarioVM Scenario { get; set; }
    }
}
