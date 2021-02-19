using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LearningPlan.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LearningPlan.DataAccess.Options;
using Microsoft.Extensions.Options;

namespace LearningPlan.DataAccess.Implementation
{
    public class ReadRepository<T> : IReadRepository<T> where T : EntityBase
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public ReadRepository(IOptions<AmazonOptions> amazonOptions)
        {
            _client = new AmazonDynamoDBClient(amazonOptions.Value.ApiKey, amazonOptions.Value.SecretKey, Amazon.RegionEndpoint.USEast2);
            _context = new DynamoDBContext(_client);
        }

        public async IAsyncEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            await foreach (var item in _context.GetTargetTable<T>().WhereDynamo(filter))
            {
                yield return _context.FromDocument<T>(item);
            }
        }

        public async IAsyncEnumerable<T> GetAll(string partitionKey)
        {
            await foreach (var item in _context.GetTargetTable<T>().WhereDynamo<T>(partitionKey)) 
            {
                yield return _context.FromDocument<T>(item);
            }
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<T>(id);
        }
    }
}