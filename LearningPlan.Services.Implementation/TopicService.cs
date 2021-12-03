using System;
using System.Collections.Generic;
using System.Globalization;
using LearningPlan.DataAccess;
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
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(
            ITopicObjectService topicObjectService,
            IUnitOfWork unitOfWork)
        {
            _topicObjectService = topicObjectService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AreaTopic> GetByIdAsync(string id)
        {
            return await _topicObjectService.GetTopicByIdAsync(id);
        }

        public async Task DeleteAsync(string topicId)
        {
            AreaTopic topic = await _topicObjectService.GetTopicByIdAsync(topicId);

            if (topic == null)
            {
                throw new DomainServicesException("Topic not found.");
            }
            
            await _topicObjectService.DeleteTopicAsync(topic);
        }

        public List<AreaTopic> GetBy(PlanArea planArea)
        {
            return _topicObjectService.GetTopicByAreaId(planArea.Id);
        }

        public async Task UpdateAsync(AreaTopicServiceModel model)
        {
            using (_unitOfWork)
            {
                var topic = await _topicObjectService.GetTopicByIdAsync(model.Id);

                if (topic == null)
                {
                    throw new DomainServicesException("Topic not found.");
                }

                topic.Name = model.Name;
                topic.StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                topic.EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                topic.Source = model.Source;
                topic.Description = model.Description;

                await _unitOfWork.CommitAsync();
            }
        }
    }
}