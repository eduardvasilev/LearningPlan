using System;
using System.Collections.Generic;
using System.Globalization;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using System.Threading.Tasks;
using LearningPlan.ObjectServices;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly ITopicObjectService _topicObjectService;

        public TopicService(
            ITopicObjectService topicObjectService)
        {
            _topicObjectService = topicObjectService;
        }

        public async Task<AreaTopic> GetByIdAsync(string id)
        {
            return await _topicObjectService.GetByIdAsync<AreaTopic>(id);
        }

        public async Task DeleteAsync(string topicId)
        {
            AreaTopic topic = await _topicObjectService.GetByIdAsync<AreaTopic>(topicId);

            if (topic == null)
            {
                throw new DomainServicesException("Topic not found.");
            }
            
            await _topicObjectService.DeleteAsync(topic);
        }

        public List<AreaTopic> GetBy(PlanArea planArea)
        {
            return _topicObjectService.GetTopicsByAreaId(planArea.Id);
        }

        public async Task UpdateAsync(AreaTopicServiceModel model)
        {
            var topic = await _topicObjectService.GetByIdAsync<AreaTopic>(model.Id);

            if (topic == null)
            {
                throw new DomainServicesException("Topic not found.");
            }

            topic.Name = model.Name;
            if (!model.IsTemplate && !string.IsNullOrEmpty(model.StartDate) && !string.IsNullOrEmpty(model.EndDate))
            {
                topic.StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                topic.EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
            }

            topic.Source = model.Source;
            topic.Description = model.Description;

            await _topicObjectService.UpdateAsync(topic);

        }
    }
}