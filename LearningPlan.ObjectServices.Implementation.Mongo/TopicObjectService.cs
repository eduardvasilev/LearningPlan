using System.Collections.Generic;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class TopicObjectService : ObjectService, ITopicObjectService
    {
        private readonly IMongoDatabase _database;

        public override string CollectionName => "topics";

        public TopicObjectService(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public async Task<AreaTopic> GetTopicByIdAsync(string id)
        {
            return await GetByIdAsync<AreaTopic>(id);
        }

        public List<AreaTopic> GetTopicByAreaId(string areaId)
        {
            return _database.GetCollection<AreaTopic>(CollectionName)
                .Find(Builders<AreaTopic>.Filter.Eq(topic => topic.PlanAreaId, areaId)).ToList();
        }

        public async Task DeleteTopicAsync(AreaTopic topic)
        {
            await _database.GetCollection<AreaTopic>(CollectionName).DeleteOneAsync(topic.ToBsonDocument());
        }
    }
}