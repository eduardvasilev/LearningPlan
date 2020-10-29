using LearningPlan.DomainModel;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services
{
    public interface ITopicService
    {
        Task DeleteAsync(string topicId);

        IQueryable<AreaTopic> GetBy(PlanArea planArea);

        Task UpdateAsync(AreaTopicServiceModel model);

    }
}