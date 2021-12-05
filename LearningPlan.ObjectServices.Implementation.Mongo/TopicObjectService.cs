using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
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

        public List<AreaTopic> GetTopicsByAreaId(string areaId)
        {
            return _database.GetCollection<AreaTopic>(CollectionName)
                .Find(Builders<AreaTopic>.Filter.Eq(topic => topic.PlanAreaId, areaId)).ToList();
        }


        public IEnumerable<AreaTopic> GetTopicsByPlanForToday(string planId, DateTime today)
        {
            return _database.GetCollection<AreaTopic>(CollectionName).Find(Builders<AreaTopic>
                                                                                     .Filter.Eq(areaTopic => areaTopic.PlanId, planId)
                                                                                 & Builders<AreaTopic>.Filter.Gte(areaTopic => areaTopic.StartDate, today)
                                                                                 & Builders<AreaTopic>.Filter.Lte(areaTopic => areaTopic.EndDate, today)).ToList();

        }
    }
}