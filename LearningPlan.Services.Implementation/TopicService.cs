using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicWriteRepository;

        public TopicService(
            IHttpContextAccessor httpContextAccessor,
            IReadRepository<AreaTopic> areaTopicReadRepository,
            IWriteRepository<AreaTopic> areaTopicWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _areaTopicReadRepository = areaTopicReadRepository;
            _areaTopicWriteRepository = areaTopicWriteRepository;
        }

        public async Task DeleteAsync(string topicId)
        {
            AreaTopic topic = await _areaTopicReadRepository.GetByIdAsync(topicId);
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            if (topic.PlanArea.Plan.UserId != user.Id)
            {
                throw new DomainServicesException("You have no permissions to delete this area topic.");
            }

            await _areaTopicWriteRepository.DeleteAsync(topic);
            await _areaTopicWriteRepository.SaveChangesAsync();
        }
    }
}