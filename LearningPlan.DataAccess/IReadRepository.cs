using LearningPlan.DomainModel;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess
{
    public interface IReadRepository<T> where T : EntityBase
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(string id);
    }
}