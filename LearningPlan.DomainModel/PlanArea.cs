using System.Collections.Generic;

namespace LearningPlan.DomainModel
{
    public class PlanArea : EntityBase
    {
        public string Name { get; set; }
        
        public string PlanId { get; set; }

        public string UserId { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual ICollection<AreaTopic> AreaTopics { get; set; } = new HashSet<AreaTopic>();
    }
}