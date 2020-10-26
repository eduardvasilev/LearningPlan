using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningPlan.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;

        public TopicService(IReadRepository<AreaTopic> areaTopicReadRepository)
        {
            _areaTopicReadRepository = areaTopicReadRepository;
        }

        public IEnumerable<AreaTopicServiceModel> GetActualTopics(string planId)
        {
            DateTime today = DateTime.Today;

            return _areaTopicReadRepository.GetAll()
                .Where(x => x.PlanId == planId)
                .Where(x =>
                    x.StartDate <= today && x.EndDate >= today)
                .Select(x => new AreaTopicServiceModel
                {
                    Name = x.Name,
                    StartDate = x.StartDate.ToString("d"),
                    EndDate = x.EndDate.ToString("d"),
                    Source = x.Source,
                })
                .ToList();
        }
    }
}