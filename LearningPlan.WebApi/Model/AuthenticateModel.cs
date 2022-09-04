using System.ComponentModel.DataAnnotations;

namespace LearningPlan.WebApi.Model
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}