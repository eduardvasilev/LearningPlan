using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LearningPlan.DomainModel;

namespace LearningPlan.DataAccess.Implementation
{
    public class WriteRepository<T> : IWriteRepository<T> where T : EntityBase
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public WriteRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await _context.SaveAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _context.SaveAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _context.DeleteAsync(entity);
        }
    }
}