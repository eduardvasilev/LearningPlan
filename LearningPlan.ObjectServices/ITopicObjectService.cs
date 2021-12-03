using System.Collections.Generic;
using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface ITopicObjectService : IObjectService
    {
        Task<AreaTopic> GetTopicByIdAsync(string id);
        List<AreaTopic> GetTopicByAreaId(string areaId);

        Task DeleteTopicAsync(AreaTopic topic);

    }
}