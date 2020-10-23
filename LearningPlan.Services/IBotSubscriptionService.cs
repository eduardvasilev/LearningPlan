using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IBotSubscriptionService
    {
        Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceServiceModel model);
    }
}