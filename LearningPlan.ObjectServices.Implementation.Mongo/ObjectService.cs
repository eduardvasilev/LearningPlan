using System.Threading.Tasks;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public abstract class ObjectService : IObjectService
    {
        private readonly IMongoDatabase _database;
        public abstract string CollectionName { get; }

        protected ObjectService(IMongoDatabase database)
        {
            _database = database;
        }

        internal async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
        {
            return await Task.FromResult(_database.GetCollection<T>(CollectionName)
                .Find(Builders<T>.Filter.Eq(topic => topic.Id, id)).FirstOrDefault());
        }
    }
}