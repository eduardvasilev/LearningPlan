using Amazon.DynamoDBv2.DataModel;

namespace LearningPlan.DomainModel
{
    public class EntityBase
    {
        [DynamoDBProperty("Id")]
        [DynamoDBHashKey]
        public string Id { get; set; }
    }
}