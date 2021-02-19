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

        [DynamoDBProperty("UserId")]
        public string UserId { get; set; }
    }
}