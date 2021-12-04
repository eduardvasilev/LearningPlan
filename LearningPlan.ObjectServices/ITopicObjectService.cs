using System;
using LearningPlan.DomainModel;
using System.Collections.Generic;

namespace LearningPlan.ObjectServices
{
    public interface ITopicObjectService : IObjectService
    {
        List<AreaTopic> GetTopicByAreaId(string areaId);
        IEnumerable<AreaTopic> GetTopicsByPlanForToday(string planId, DateTime today);
    }
}