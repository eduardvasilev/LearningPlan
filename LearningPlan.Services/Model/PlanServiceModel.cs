using System.ComponentModel.DataAnnotations;

namespace LearningPlan.Services.Model
{
    public class PlanServiceModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string PlanAreaName { get; set; }

        public AreaTopicServiceModel[] AreaTopics { get; set; }
    }
}