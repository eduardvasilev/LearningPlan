using System.Collections.Generic;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IPlanObjectService : IObjectService
    {
        IEnumerable<Plan> GetUserPlans(string userId);
        IEnumerable<Plan> GetTemplatePlans();
    }
}