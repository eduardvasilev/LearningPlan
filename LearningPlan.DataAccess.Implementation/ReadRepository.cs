using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LearningPlan.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess.Implementation
{
    public class ReadRepository<T> : IReadRepository<T> where T : EntityBase
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public ReadRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            var table = _context.GetTargetTable<T>();
            List<Document> data;
            if (filter != null) {
                data = table.WhereDynamo<T>(x => x.Id == "");
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