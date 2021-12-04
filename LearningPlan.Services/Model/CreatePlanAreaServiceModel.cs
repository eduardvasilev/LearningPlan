using System.ComponentModel.DataAnnotations;

namespace LearningPlan.Services.Model
{
    public class CreatePlanAreaServiceModel
    {
        [Required]
        public string PlanId { get; set; }
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}