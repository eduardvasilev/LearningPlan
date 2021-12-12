using System.Threading.Tasks;
using LearningPlan.DomainModel;
using MongoDB.Bson;
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

        public async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
        {
            return await Task.FromResult(_database.GetCollection<T>(CollectionName)
                .Find(Builders<T>.Filter.Eq(topic => topic.Id, id)).FirstOrDefault());
        }

        public async Task CreateAsync<T>(T entity) where T: EntityBase
        {
            await _database.GetCollection<T>(CollectionName).InsertOneAsync(entity);
        }

        public async Task DeleteAsync<T>(T entity) where T: EntityBase
        {
            await _database.GetCollection<T>(CollectionName).DeleteOneAsync(entity.ToBsonDocument());
        }

        public async Task UpdateAsync<T>(T entity) where T : EntityBase
        {
            await _database.GetCollection<T>(CollectionName).ReplaceOneAsync(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity);
        }
    }
}