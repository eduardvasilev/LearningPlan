using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IWriteRepository<Plan> _writeRepository;

        public PlanService(IWriteRepository<Plan> writeRepository)
        {
            _writeRepository = writeRepository;
        }

        public async Task CreatePlanAsync(string name)
        {
            await _writeRepository.CreateAsync(new Plan(name));
            await _writeRepository.SaveChangesAsync();
        }
    }
}
