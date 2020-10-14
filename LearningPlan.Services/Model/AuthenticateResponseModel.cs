namespace LearningPlan.Services.Model
{
    public class AuthenticateResponseModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponseModel(string userId, string username, string token)
        {
            Id = userId;
            Username = username;
            Token = token;
        }
    }
}