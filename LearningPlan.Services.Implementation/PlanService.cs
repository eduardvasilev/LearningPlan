using System;
using System.Globalization;
using System.Linq;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using System.Threading.Tasks;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IReadRepository<Plan> _planReadRepository;
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<Plan> _planRepository;
        private readonly IWriteRepository<PlanArea> _planAreaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IReadRepository<Plan> planReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            IWriteRepository<AreaTopic> areaTopicRepository,
            IWriteRepository<Plan> planRepository,
            IWriteRepository<PlanArea> planAreaRepository,
            IUnitOfWork unitOfWork)
        {
            _planReadRepository = planReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _areaTopicRepository = areaTopicRepository;
            _planRepository = planRepository;
            _planAreaRepository = planAreaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePlanAsync(PlanServiceModel model)
        {
            using (_unitOfWork)
            {
                Plan plan = new Plan(model.Name);
                await _planRepository.CreateAsync(plan);

                PlanArea planArea = new PlanArea
                {
                    Name = model.PlanAreaName,
                    Plan = plan,
                    PlanId = plan.Id
                };

                await _planAreaRepository.CreateAsync(planArea);
                foreach (AreaTopicServiceModel areaTopic in model.AreaTopics)
                {
                    await _areaTopicRepository.CreateAsync(new AreaTopic
                    {
                        Name = areaTopic.Name,
                        StartDate = DateTime.ParseExact(areaTopic.StartDate, "dd/mm/yyyy", CultureInfo.CurrentCulture),
                        EndDate = DateTime.ParseExact(areaTopic.EndDate, "dd/mm/yyyy", CultureInfo.CurrentCulture),
                        Source = areaTopic.Source,
                        PlanArea = planArea,
                        PlanAreaId = planArea.Id
                    });
                }

                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<PlanServiceModel> GetById(string id)
        {
            Plan plan = await _planReadRepository.GetByIdAsync(id);
            PlanArea planArea = _planAreaReadRepository.GetAll().FirstOrDefault(x => x.PlanId == id);
            return await Task.FromResult(new PlanServiceModel
            {
                Name = plan.Name,
                PlanAreaName = planArea?.Name,
                AreaTopics = planArea?.AreaTopics.Select(x => new AreaTopicServiceModel
                {
                    Name = x.Name,
                    StartDate = x.StartDate.ToString("d"),
                    EndDate = x.EndDate.ToString("d"),
                    Source = x.Source
                }).ToArray()
            });
        }
    }
}
