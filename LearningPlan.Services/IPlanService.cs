using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model);

        Task<PlanServiceModel> GetById(string id);
    }
}