using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanAreaService
    {
        Task<PlanAreaServiceModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model);
        Task<AreaTopicResponseModel> CreateAreaTopicAsync(CreateAreaTopicServiceModel model);
        Task DeleteAsync(string id);
        IQueryable<PlanArea> GetBy(Plan plan);
        Task UpdateAsync(PlanAreaServiceModel model);
    }
}