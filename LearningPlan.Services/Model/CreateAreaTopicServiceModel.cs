using System;

namespace LearningPlan.Services.Model
{
    public class CreateAreaTopicServiceModel
    {
        public string PlanAreaId { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Description { get; set; }

        public bool IsTemplate { get; set; }
    }
}