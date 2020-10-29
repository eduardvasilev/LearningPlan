using System;
using System.Globalization;
using System.Linq;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(
            IHttpContextAccessor httpContextAccessor,
            IReadRepository<AreaTopic> areaTopicReadRepository,
            IWriteRepository<AreaTopic> areaTopicWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _areaTopicReadRepository = areaTopicReadRepository;
            _areaTopicWriteRepository = areaTopicWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteAsync(string topicId)
        {
            AreaTopic topic = await _areaTopicReadRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new DomainServicesException("Topic not found.");
            }

            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            if (topic.PlanArea.Plan.UserId != user.Id)
            {
                throw new DomainServicesException("You have no permissions to delete this area topic.");
            }

            await _areaTopicWriteRepository.DeleteAsync(topic);
            await _areaTopicWriteRepository.SaveChangesAsync();
        }

        public IQueryable<AreaTopic> GetBy(PlanArea planArea)
        {
            return _areaTopicReadRepository.GetAll().Where(topic => topic.PlanAreaId == planArea.Id);
        }

        public async Task UpdateAsync(AreaTopicServiceModel model)
        {
            using (_unitOfWork)
            {
                var topic = await _areaTopicReadRepository.GetByIdAsync(model.Id);

                if (topic == null)
                {
                    throw new DomainServicesException("Topic not found.");
                }

                User user = (User)_httpContextAccessor.HttpContext.Items["User"];
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                if (topic.PlanArea.Plan.UserId != user.Id)
                {
                    throw new DomainServicesException("You have no permissions to delete this area topic.");
                }

                topic.Name = model.Name;
                topic.StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                topic.EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                topic.Source = model.Source;

                await _unitOfWork.CommitAsync();
            }
        }
    }
}