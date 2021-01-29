using Amazon.DynamoDBv2.DataModel;

namespace LearningPlan.DomainModel
{
    [DynamoDBTable("Users")]
    public class User : EntityBase
    {
        [DynamoDBProperty("Username")]
        public string Username { get; set; }

        [DynamoDBProperty("Password")]
        public string Password { get; set; }

        [DynamoDBProperty("Salt")]
        public byte[] Salt { get; set; }
    }
}