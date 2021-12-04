using System;

namespace LearningPlan.DomainModel
{
    public class AreaTopic : EntityBase
    {
        public string Name { get; set; }

        public string Source { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual PlanArea PlanArea { get; set; }

        public string PlanAreaId { get; set; }

        public string PlanId { get; set; }
        public string UserId { get; set; }

        public string Description { get; set; }
    }
}