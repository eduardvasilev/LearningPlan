using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task CreatePlanAsync(PlanServiceModel model);
        Task<PlanServiceModel> GetById(long id);
    }
}