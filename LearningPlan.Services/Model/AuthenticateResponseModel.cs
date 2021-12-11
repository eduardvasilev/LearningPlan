namespace LearningPlan.Services.Model
{
    public class AuthenticateResponseModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponseModel(string userId, string username, string token)
        {
            UserId = userId;
            Username = username;
            Token = token;
        }
    }
}