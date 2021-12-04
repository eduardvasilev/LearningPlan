using System.Collections.Generic;
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
        Task<PlanArea> GetByIdAsync(string id);
        IEnumerable<PlanArea> GetBy(Plan plan);
        Task UpdateAsync(PlanAreaServiceModel model);
    }
}