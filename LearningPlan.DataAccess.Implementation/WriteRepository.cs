using System;
using System.Threading.Tasks;
using LearningPlan.DomainModel;

namespace LearningPlan.DataAccess.Implementation
{
    public class WriteRepository<T> : IWriteRepository<T> where T : EntityBase
    {
        private readonly EfContext _context;

        public WriteRepository(EfContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await _context.Set<T>().AddAsync(entity);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}