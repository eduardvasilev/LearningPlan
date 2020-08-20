using System;
using System.Threading.Tasks;

namespace LearningPlan.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
    }
}