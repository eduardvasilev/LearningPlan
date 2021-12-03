using System.Collections.Generic;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IPlanObjectService : IObjectService
    {
        List<Plan> GetUserPlans(string userId);
    }
}