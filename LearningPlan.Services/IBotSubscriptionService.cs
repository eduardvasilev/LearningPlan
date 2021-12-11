using LearningPlan.Services.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.Services
{
    public interface IBotSubscriptionService
    {
        Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceModel model);
        IEnumerable<BotSubscriptionServiceModel> GetAll();
        IEnumerable<AreaTopicServiceModel> GetActualTopics(string planId);
    }
}