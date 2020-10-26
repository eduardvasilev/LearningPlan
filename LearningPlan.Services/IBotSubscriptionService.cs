﻿using System.Linq;
using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface IBotSubscriptionService
    {
        Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceModel model);
        IQueryable<BotSubscriptionServiceModel> GetAll();
    }
}