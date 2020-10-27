using LearningPlan.Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningPlan.Services
{
    public interface ITopicService
    {
        Task DeleteAsync(string topicId);
    }
}