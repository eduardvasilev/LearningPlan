using System.Collections.Generic;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IPlanAreaObjectService : IObjectService
    {
        IEnumerable<PlanArea> GetPlanAreas(string planId);
    }
}