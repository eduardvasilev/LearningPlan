using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace LearningPlan.DomainModel
{
    [DynamoDBTable("PlanAreas")]
    public class PlanArea : EntityBase
    {
        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("PlanId")]
        public string PlanId { get; set; }

        [DynamoDBProperty("Plan")]
        public virtual Plan Plan { get; set; }

        [DynamoDBProperty("AreaTopics")]
        public virtual ICollection<AreaTopic> AreaTopics { get; set; } = new HashSet<AreaTopic>();
    }
}