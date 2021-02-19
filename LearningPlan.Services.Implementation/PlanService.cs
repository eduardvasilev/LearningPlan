using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;
        private readonly IPlanAreaService _planAreaService;
        private readonly IReadRepository<BotSubscription> _botSubscriptionReadRepository;
        private readonly IReadRepository<Plan> _planReadRepository;
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<BotSubscription> _botSubscriptionWriteRepository;
        private readonly IWriteRepository<Plan> _planWriteRepository;
        private readonly IWriteRepository<PlanArea> _planAreaRepository;

        public PlanService(
            IReadRepository<AreaTopic> areaTopicReadRepository,
            IPlanAreaService planAreaService,
            IReadRepository<BotSubscription> botSubscriptionReadRepository,
            IReadRepository<Plan> planReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            IWriteRepository<AreaTopic> areaTopicRepository,
            IWriteRepository<BotSubscription> botSubscriptionWriteRepository,
            IWriteRepository<Plan> planWriteRepository,
            IWriteRepository<PlanArea> planAreaRepository)
        {
            _areaTopicReadRepository = areaTopicReadRepository;
            _planAreaService = planAreaService;
            _botSubscriptionReadRepository = botSubscriptionReadRepository;
            _planReadRepository = planReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _areaTopicRepository = areaTopicRepository;
            _botSubscriptionWriteRepository = botSubscriptionWriteRepository;
            _planWriteRepository = planWriteRepository;
            _planAreaRepository = planAreaRepository;
        }

        public async Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model)
        {

            var plan = new Plan()
            {
                Name = model.Name,
                UserId = model.UserId
            };

            await _planWriteRepository.CreateAsync(plan);

            foreach (PlanAreaServiceModel planArea in model.PlanAreas)
            {
                PlanArea area = new PlanArea
                {
                    Name = planArea.Name,
                    PlanId = plan.Id,
                    UserId = plan.UserId
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
                        UserId = plan.UserId,
                        PlanAreaId = area.Id
                    });
                }
            }

            return new PlanResponseModel
            {
                Name = plan.Name,
                Id = plan.Id
            };
        }

        public async Task DeleteAsync(string planId)
        {

            Plan plan = await _planReadRepository.GetByIdAsync(planId);

            if (plan == null)
            {
                throw new DomainServicesException("Plan not found.");
            }

            foreach (PlanArea planArea in _planAreaService.GetBy(plan))
            {
                await _planAreaService.DeleteAsync(planArea.Id);
            }

            foreach (BotSubscription botSubscription in _botSubscriptionReadRepository.GetAll(s => s.PlanId == planId))
            {
                await _botSubscriptionWriteRepository.DeleteAsync(botSubscription);
            }

            await _planWriteRepository.DeleteAsync(plan);

        }

        public async Task<PlanServiceModel> GetByIdAsync(string id)
        {
            Plan plan = await _planReadRepository.GetByIdAsync(id);
            var planAreas = _planAreaReadRepository.GetAll(x => x.PlanId == id).ToList();

            IEnumerable<AreaTopic> areaTopics = _areaTopicReadRepository.GetAll(x => x.PlanId == id);

            PlanServiceModel planServiceModel = new PlanServiceModel
            {
                Id = plan.Id,
                Name = plan.Name,
                UserId = plan.UserId,
                PlanAreas = planAreas.Select(x => new PlanAreaServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PlanId = x.PlanId,
                    AreaTopics = areaTopics.Select(areaTopic => new AreaTopicServiceModel
                    {
                        Id = areaTopic.Id,
                        Name = areaTopic.Name,
                        StartDate = areaTopic.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = areaTopic.EndDate.ToString("yyyy-MM-dd"),
                        Source = areaTopic.Source,
                        Description = areaTopic.Description
                    }).OrderBy(topic => topic.StartDate).ToArray()
                }).ToArray()
            };
            return await Task.FromResult(planServiceModel);
        }

        public IEnumerable<PlanResponseModel> GetAll(User user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return _planReadRepository.GetAll(x => x.UserId == user.Id)
                .Select(plan => new PlanResponseModel
                {
                    Id = plan.Id,
                    Name = plan.Name
                });
        }

        public async Task UpdateAsync(PlanServiceModel model)
        {

            Plan plan = await _planReadRepository.GetByIdAsync(model.Id);

            if (plan == null)
            {
                throw new DomainServicesException("Plan not found.");
            }

            plan.Name = model.Name;
            await _planWriteRepository.UpdateAsync(plan);
        }
    }
}
