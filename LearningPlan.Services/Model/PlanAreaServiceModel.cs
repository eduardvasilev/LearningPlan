using System.ComponentModel.DataAnnotations;

namespace LearningPlan.Services.Model
{
    public class PlanAreaServiceModel
    {
        public string Id { get; set; }

        public string PlanId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public AreaTopicServiceModel[] AreaTopics { get; set; }
    }
}