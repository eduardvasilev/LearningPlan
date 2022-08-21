namespace LearningPlan.DomainModel
{
    public class User : EntityBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public bool IsApproved { get; set; }
    }
}