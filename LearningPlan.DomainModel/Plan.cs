using Amazon.DynamoDBv2.DataModel;

namespace LearningPlan.DomainModel
{
    [DynamoDBTable("Plans")]
    public class Plan : EntityBase
    {
        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("UserId")]
        public string UserId { get; set; }
    }
}
