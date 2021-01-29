using Amazon.DynamoDBv2.DataModel;

namespace LearningPlan.DomainModel
{
    [DynamoDBTable("BotSubscriptions")]
    public class BotSubscription : EntityBase
    {
        [DynamoDBProperty("ChatId")]
        public string ChatId { get; set; }

        [DynamoDBProperty("PlanId")]
        public string PlanId { get; set; }

        [DynamoDBProperty("Plan")]
        public virtual Plan Plan { get; set; }
    }
}