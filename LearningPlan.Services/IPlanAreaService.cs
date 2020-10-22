using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanAreaService
    {
        Task<PlanAreaServiceModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model);
        Task CreateAreaTopicAsync(CreateAreaTopicServiceModel model);
    }
}