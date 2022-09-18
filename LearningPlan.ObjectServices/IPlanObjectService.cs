using System.Collections.Generic;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IPlanObjectService : IObjectService
    {
        IEnumerable<Plan> GetUserPlans(string userId, bool templates = false);
        IEnumerable<Plan> GetTemplatePlans();
    }
}