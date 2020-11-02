using System.ComponentModel.DataAnnotations;

namespace LearningPlan.Services.Model
{
    public class PlanServiceModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public PlanAreaServiceModel[] PlanAreas { get; set; }

        public string UserId { get; set; }
    }
}