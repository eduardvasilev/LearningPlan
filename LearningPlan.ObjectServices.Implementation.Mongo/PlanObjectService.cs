using System.Collections.Generic;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class PlanObjectService : ObjectService, IPlanObjectService
    {
        private readonly IMongoDatabase _database;
        public override string CollectionName { get => "plans"; }

        public PlanObjectService(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public List<Plan> GetUserPlans(string userId)
        {
           return _database.GetCollection<Plan>(CollectionName).Find(Builders<Plan>.Filter.Eq(plan => plan.UserId, userId)).ToList();
        }
    }
}