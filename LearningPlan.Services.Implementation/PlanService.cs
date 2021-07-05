using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IPlanAreaService _planAreaService;
        private readonly IReadRepository<BotSubscription> _botSubscriptionReadRepository;
        private readonly IReadRepository<Plan> _planReadRepository;
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<BotSubscription> _botSubscriptionWriteRepository;
        private readonly IWriteRepository<Plan> _planWriteRepository;
        private readonly IWriteRepository<PlanArea> _planAreaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IPlanAreaService planAreaService,
            IReadRepository<BotSubscription> botSubscriptionReadRepository,
            IReadRepository<Plan> planReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            IWriteRepository<AreaTopic> areaTopicRepository,
            IWriteRepository<BotSubscription> botSubscriptionWriteRepository,
            IWriteRepository<Plan> planWriteRepository,
            IWriteRepository<PlanArea> planAreaRepository,
            IUnitOfWork unitOfWork)
        {
            _planAreaService = planAreaService;
            _botSubscriptionReadRepository = botSubscriptionReadRepository;
            _planReadRepository = planReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _areaTopicRepository = areaTopicRepository;
            _botSubscriptionWriteRepository = botSubscriptionWriteRepository;
            _planWriteRepository = planWriteRepository;
            _planAreaRepository = planAreaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model)
        {
            using (_unitOfWork)
            {
                var plan = new Plan(model.Name, model.UserId);

                await _planWriteRepository.CreateAsync(plan);

                foreach (PlanAreaServiceModel planArea in model.PlanAreas)
                {
                    PlanArea area = new PlanArea
                    {
                        Name = planArea.Name,
                        Plan = plan,
                        PlanId = plan.Id
                    };

                    await _planAreaRepository.CreateAsync(area);
                    foreach (AreaTopicServiceModel areaTopic in planArea.AreaTopics)
                    {
                        await _areaTopicRepository.CreateAsync(new AreaTopic
                        {
                            Name = areaTopic.Name,
                            StartDate = DateTime.ParseExact(areaTopic.StartDate, "yyyy-MM-dd",
                                CultureInfo.CurrentCulture),
                            EndDate = DateTime.ParseExact(areaTopic.EndDate, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                            Source = areaTopic.Source,
                            Description = areaTopic.Description,
                            PlanArea = area,
                            PlanAreaId = area.Id
                        });
                    }
                }

                await _unitOfWork.CommitAsync();

                return new PlanResponseModel
                {
                    Name = plan.Name,
                    Id = plan.Id
                };
            }
        }

        public async Task DeleteAsync(string planId)
        {

            Plan plan = await _planReadRepository.GetByIdAsync(planId);

            if (plan == null)
            {
                throw new DomainServicesException("Plan not found.");
            }

            foreach (PlanArea planArea in _planAreaService.GetBy(plan).ToList())
            {
                await _planAreaService.DeleteAsync(planArea.Id);
            }

            foreach (BotSubscription botSubscription in _botSubscriptionReadRepository.GetAll().Where(s => s.PlanId == planId).ToList())
            {
                await _botSubscriptionWriteRepository.DeleteAsync(botSubscription);
            }

            await _planWriteRepository.DeleteAsync(plan);

            await _unitOfWork.CommitAsync();
    }

        public async Task<PlanServiceModel> GetByIdAsync(string id)
        {
            Plan plan = await _planReadRepository.GetByIdAsync(id);
            var planAreas = _planAreaReadRepository.GetAll().Where(x => x.PlanId == id).ToList();
           
            return await Task.FromResult(new PlanServiceModel
            {
                Id = plan.Id,
                Name = plan.Name,
                UserId = plan.UserId,
                PlanAreas = planAreas.Select(x => new PlanAreaServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PlanId = x.PlanId,
                    AreaTopics = x.AreaTopics.Select(areaTopic => new AreaTopicServiceModel
                    {
                        Id = areaTopic.Id,
                        Name = areaTopic.Name,
                        StartDate = areaTopic.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = areaTopic.EndDate.ToString("yyyy-MM-dd"),
                        Source = areaTopic.Source,
                        Description = areaTopic.Description
                    }).OrderBy(topic => topic.StartDate).ToArray()
                }).ToArray()
            });
        }

        public IQueryable<PlanResponseModel> GetAll(User user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return _planReadRepository.GetAll().Where(x => x.UserId == user.Id)
                .Select(plan => new PlanResponseModel
                {
                    Id = plan.Id,
                    Name = plan.Name
                });
        }

        public async Task UpdateAsync(PlanServiceModel model)
        {
            using (_unitOfWork)
            {

                Plan plan = await _planReadRepository.GetByIdAsync(model.Id);

                if (plan == null)
                {
                    throw new DomainServicesException("Plan not found.");
                }

                plan.Name = model.Name;

                await _unitOfWork.CommitAsync();
            }
        }
    }
}
