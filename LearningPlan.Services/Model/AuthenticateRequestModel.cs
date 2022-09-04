namespace LearningPlan.Services.Model
{
    public class AuthenticateRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Secret { get; set; }
    }
}