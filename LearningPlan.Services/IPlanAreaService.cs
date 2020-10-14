using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanAreaService
    {
        Task<AreaTopicResponseModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model);
        Task CreateAreaTopicAsync(CreateAreaTopicServiceModel model);
    }
}