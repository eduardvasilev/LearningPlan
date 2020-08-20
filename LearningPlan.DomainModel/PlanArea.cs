using System.Collections.Generic;

namespace LearningPlan.DomainModel
{
    public class PlanArea : EntityBase
    {
        public string Name { get; set; }
        
        public long? PlanId { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual ICollection<AreaTopic> AreaTopics { get; set; } = new HashSet<AreaTopic>();
    }
}