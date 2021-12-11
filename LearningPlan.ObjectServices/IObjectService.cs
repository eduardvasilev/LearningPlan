using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.ObjectServices
{
    public interface IObjectService
    {
        public string CollectionName { get; }

        Task<T> GetByIdAsync<T>(string id) where T : EntityBase;

        Task CreateAsync<T>(T entity) where  T : EntityBase;

        Task DeleteAsync<T>(T entity) where  T : EntityBase;

        Task UpdateAsync<T>(T entity) where T : EntityBase;

    }
}