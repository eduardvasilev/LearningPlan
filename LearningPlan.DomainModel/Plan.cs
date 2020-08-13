using System;

namespace LearningPlan.DomainModel
{
    public class Plan : EntityBase
    {
        public Plan(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

    }
}
