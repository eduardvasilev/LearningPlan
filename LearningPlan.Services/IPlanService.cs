using System.Linq;
using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model);
        Task DeleteAsync(string planId);

        Task<PlanServiceModel> GetByIdAsync(string id);

        IQueryable<PlanResponseModel> GetAll();

        Task UpdateAsync(PlanServiceModel model);
    }
}