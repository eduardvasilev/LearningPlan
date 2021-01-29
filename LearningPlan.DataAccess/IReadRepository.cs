using LearningPlan.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess
{
    public interface IReadRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);

        Task<T> GetByIdAsync(string id);
    }
}