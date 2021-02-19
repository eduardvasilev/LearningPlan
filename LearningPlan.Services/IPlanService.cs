﻿using LearningPlan.Services.Model;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using System.Collections.Generic;

namespace LearningPlan.Services
{
    public interface IPlanService
    {
        Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model);
        Task DeleteAsync(string planId);

        Task<PlanServiceModel> GetByIdAsync(string id);

        IAsyncEnumerable<PlanResponseModel> GetAll(User user);

        Task UpdateAsync(PlanServiceModel model);
    }
}