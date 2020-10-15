using System.ComponentModel.DataAnnotations;

namespace LearningPlan.WebApi.Model
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}