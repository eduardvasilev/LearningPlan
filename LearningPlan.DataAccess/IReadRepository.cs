using LearningPlan.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess
{
    public interface IReadRepository<T> where T : EntityBase
    {
        IAsyncEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);

        IAsyncEnumerable<T> GetAll(string partitionKey);

        Task<T> GetByIdAsync(string id);
    }
}