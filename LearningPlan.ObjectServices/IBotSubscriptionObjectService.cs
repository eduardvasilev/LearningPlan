using System;
using System.Collections.Generic;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IBotSubscriptionObjectService : IObjectService
    {
        IEnumerable<BotSubscription> GetAll();
        IEnumerable<BotSubscription> GetBotSubscriptionsByPlan(string planId);
        IEnumerable<BotSubscription> GetBotSubscriptionsByPlanAndChat(string planId, string chatId);
    }
}