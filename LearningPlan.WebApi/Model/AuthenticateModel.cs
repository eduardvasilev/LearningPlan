using System.ComponentModel.DataAnnotations;

namespace LearningPlan.WebApi.Model
{
    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}