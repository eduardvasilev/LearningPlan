using System;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class UserObjectService : ObjectService, IUserObjectService
    {
        private readonly IMongoDatabase _database;
        public override string CollectionName { get => "users"; }

        public UserObjectService(IMongoDatabase database) : base(database)
        {
            _database = database;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await Task.FromResult(_database.GetCollection<User>(CollectionName)
                .Find(Builders<User>.Filter.Eq(user => user.Username, username)).FirstOrDefault());
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await GetByIdAsync<User>(id);

        }

        public async Task CreateUserAsync(User user)
        {
            await _database.GetCollection<User>(CollectionName).InsertOneAsync(user);
        }

    }
}
