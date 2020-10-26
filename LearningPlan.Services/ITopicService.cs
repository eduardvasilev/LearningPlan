using LearningPlan.Services.Model;
using System.Collections.Generic;

namespace LearningPlan.Services
{
    public interface ITopicService
    {
        IEnumerable<AreaTopicServiceModel> GetActualTopics(string planId);
    }
}