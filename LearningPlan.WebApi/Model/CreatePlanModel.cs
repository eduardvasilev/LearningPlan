using System.ComponentModel.DataAnnotations;

namespace LearningPlan.WebApi.Model
{
    public class CreatePlanModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string PlanAreaName { get; set; }




    }
}