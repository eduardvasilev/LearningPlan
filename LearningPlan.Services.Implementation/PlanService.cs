using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.ObjectServices;

namespace LearningPlan.Services.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IPlanAreaService _planAreaService;
        private readonly IReadRepository<BotSubscription> _botSubscriptionReadRepository;
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<BotSubscription> _botSubscriptionWriteRepository;
        private readonly IWriteRepository<PlanArea> _planAreaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanObjectService _planObjectService;

        public PlanService(IPlanAreaService planAreaService,
            IReadRepository<BotSubscription> botSubscriptionReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            IWriteRepository<AreaTopic> areaTopicRepository,
            IWriteRepository<BotSubscription> botSubscriptionWriteRepository,
            IWriteRepository<PlanArea> planAreaRepository,
            IUnitOfWork unitOfWork,
            IPlanObjectService planObjectService)
        {
            _planAreaService = planAreaService;
            _botSubscriptionReadRepository = botSubscriptionReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _areaTopicRepository = areaTopicRepository;
            _botSubscriptionWriteRepository = botSubscriptionWriteRepository;
            _planAreaRepository = planAreaRepository;
            _unitOfWork = unitOfWork;
            _planObjectService = planObjectService;
        }

        public async Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model)
        {
            using (_unitOfWork)
            {
                var plan = new Plan(model.Name, model.UserId);

                await _planObjectService.CreateAsync(plan);

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
            using (_unitOfWork)
            {

                Plan plan = await _planObjectService.GetByIdAsync<Plan>(planId);

                if (plan == null)
                {
                    throw new DomainServicesException("Plan not found.");
                }

                foreach (PlanArea planArea in _planAreaService.GetBy(plan))
                {
                    await _planAreaService.DeleteAsync(planArea.Id);
                }

                foreach (BotSubscription botSubscription in _botSubscriptionReadRepository.GetAll().Where(s => s.PlanId == planId))
                {
                    await _botSubscriptionWriteRepository.DeleteAsync(botSubscription);
                }

                await _planObjectService.DeleteAsync(plan);

                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<PlanServiceModel> GetByIdAsync(string id)
        {
            Plan plan = await _planObjectService.GetByIdAsync<Plan>(id);
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

        public IEnumerable<PlanResponseModel> GetAll(User user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return _planObjectService.GetUserPlans(user.Id)
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

                Plan plan = await _planObjectService.GetByIdAsync<Plan>(model.Id);

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
