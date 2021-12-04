using System;
using System.Collections.Generic;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class BotSubscriptionObjectService : ObjectService, IBotSubscriptionObjectService
    {
        private readonly IMongoDatabase _database;
        public override string CollectionName { get => "botSubscriptions"; }

        public BotSubscriptionObjectService(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public IEnumerable<BotSubscription> GetAll()
        {
            return _database.GetCollection<BotSubscription>(CollectionName).Find(_ => true).ToList();
        }

        public IEnumerable<BotSubscription> GetBotSubscriptionsByPlan(string planId)
        {
            return _database.GetCollection<BotSubscription>(CollectionName).Find(Builders<BotSubscription>.Filter.Eq(botSubscription => botSubscription.PlanId, planId)).ToList();
        }

        public IEnumerable<BotSubscription> GetBotSubscriptionsByPlanAndChat(string planId, string chatId)
        {
            return _database.GetCollection<BotSubscription>(CollectionName).Find(Builders<BotSubscription>
                .Filter.Eq(botSubscription => botSubscription.PlanId, planId) & Builders<BotSubscription>.Filter.Eq(subscription => subscription.ChatId, chatId)).ToList();
        }

    }
}