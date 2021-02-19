using Amazon.DynamoDBv2.DataModel;
using System;

namespace LearningPlan.DomainModel
{
    [DynamoDBTable("AreaTopics")]
    public class AreaTopic : EntityBase
    {
        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("Source")]
        public string Source { get; set; }

        [DynamoDBProperty("StartDate")]
        public DateTime StartDate { get; set; }

        [DynamoDBProperty("EndDate")]
        public DateTime EndDate { get; set; }

        [DynamoDBProperty("PlanAreaId")]
        public string PlanAreaId { get; set; }

        [DynamoDBProperty("PlanId")]
        public string PlanId { get; set; }

        [DynamoDBProperty("Description")]
        public string Description { get; set; }

        [DynamoDBProperty("UserId")]
        public string UserId { get; set; }
    }
}