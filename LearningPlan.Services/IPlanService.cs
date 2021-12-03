using System.Collections.Generic;
using LearningPlan.Services.Model;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model);
        Task DeleteAsync(string planId);

        Task<PlanServiceModel> GetByIdAsync(string id);

        IEnumerable<PlanResponseModel> GetAll(User user);

        Task UpdateAsync(PlanServiceModel model);
    }
}