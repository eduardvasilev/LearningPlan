using LearningPlan.Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningPlan.Services
{
    public interface IBotSubscriptionService
    {
        Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceModel model);
        IAsyncEnumerable<BotSubscriptionServiceModel> GetAll();
        IAsyncEnumerable<AreaTopicServiceModel> GetActualTopics(string planId);
    }
}