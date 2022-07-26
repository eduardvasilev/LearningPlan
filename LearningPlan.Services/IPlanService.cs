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

        IEnumerable<PlanResponseModel> GetAllTemplates();

        Task UpdateAsync(PlanServiceModel model);
        Task CopyTemplatePlanAsync(string userId, string planId);
        Task SaveAsTemplateAsync(string userId, string planId);
    }
}