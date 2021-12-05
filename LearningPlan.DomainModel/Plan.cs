using System;

namespace LearningPlan.DomainModel
{
    public class Plan : EntityBase
    {
        public Plan(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }
        public string Name { get; set; }
        public string UserId { get; set; }
        public bool IsTemplate { get; set; }
    }
}
