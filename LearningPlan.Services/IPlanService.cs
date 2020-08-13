using System.Threading.Tasks;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task CreatePlanAsync(string name);
    }
}