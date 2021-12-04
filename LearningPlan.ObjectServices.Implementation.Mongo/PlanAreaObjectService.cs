using System.Collections.Generic;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class PlanAreaObjectService : ObjectService, IPlanAreaObjectService
    {
        private readonly IMongoDatabase _database;
        public override string CollectionName => "areas";

        public PlanAreaObjectService(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public IEnumerable<PlanArea> GetPlanAreas(string planId)
        {
            return _database.GetCollection<PlanArea>(CollectionName)
                .Find(Builders<PlanArea>.Filter.Eq(area => area.PlanId, planId)).ToList();

        }

    }
}