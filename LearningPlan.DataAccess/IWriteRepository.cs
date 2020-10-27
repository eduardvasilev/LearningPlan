using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.DataAccess
{
    public interface IWriteRepository<in T> where T : EntityBase
    {
        Task CreateAsync(T entity);

        Task SaveChangesAsync();

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}