using System;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using MongoDB.Driver;

namespace LearningPlan.ObjectServices.Implementation.Mongo
{
    public class UserObjectService : IUserObjectService
    {
        private readonly IMongoDatabase _database;

        public UserObjectService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await Task.FromResult(_database.GetCollection<User>("users")
                .Find(Builders<User>.Filter.Eq(user => user.Username, username)).FirstOrDefault());
        }

        public async Task<User> GetUserByIdAsync(string id)
        {

            return await Task.FromResult(_database.GetCollection<User>("users")
                .Find(Builders<User>.Filter.Eq(user => user.Id, id)).FirstOrDefault());
        }

        public async Task CreateUserAsync(User user)
        {
            await _database.GetCollection<User>("users").InsertOneAsync(user);
        }
    }
}
