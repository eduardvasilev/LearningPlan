using System;
using System.Globalization;
using System.Linq;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using System.Threading.Tasks;
using LearningPlan.Services.Model;
using System.Collections.Generic;

namespace LearningPlan.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicWriteRepository;

        public TopicService(
            IReadRepository<AreaTopic> areaTopicReadRepository,
            IWriteRepository<AreaTopic> areaTopicWriteRepository)
        {
            _areaTopicReadRepository = areaTopicReadRepository;
            _areaTopicWriteRepository = areaTopicWriteRepository;
        }

        public async Task<AreaTopic> GetByIdAsync(string id)
        {
            return await _areaTopicReadRepository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(string topicId)
        {
            AreaTopic topic = await _areaTopicReadRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new DomainServicesException("Topic not found.");
            }
            
            await _areaTopicWriteRepository.DeleteAsync(topic);
        }

        public IEnumerable<AreaTopic> GetBy(PlanArea planArea)
        {
            return _areaTopicReadRepository.GetAll(topic => topic.PlanAreaId == planArea.Id);
        }

        public async Task UpdateAsync(AreaTopicServiceModel model)
        {

            var topic = await _areaTopicReadRepository.GetByIdAsync(model.Id);

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

            await _areaTopicWriteRepository.UpdateAsync(topic);
        }
    }
}