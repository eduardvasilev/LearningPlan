using System.Threading.Tasks;

namespace LearningPlan.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfContext _context;

        public UnitOfWork(EfContext context)
        {
            _context = context;
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}