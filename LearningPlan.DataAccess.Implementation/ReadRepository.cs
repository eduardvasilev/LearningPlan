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

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            var table = _context.GetTargetTable<T>();
            List<Document> data;
            if (filter != null) {
                data = table.WhereDynamo<T>(filter);
            }
            else
            {
                var scanOps = new ScanOperationConfig();
                data = table.Scan(scanOps).GetNextSetAsync().GetAwaiter().GetResult();
            }
            var result = _context.FromDocuments<T>(data);
            return result;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<T>(id);
        }
    }
}