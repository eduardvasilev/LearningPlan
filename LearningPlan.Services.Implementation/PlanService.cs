using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.ObjectServices;
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
        private readonly IPlanAreaService _planAreaService;
        private readonly IPlanObjectService _planObjectService;
        private readonly IPlanAreaObjectService _planAreaObjectService;
        private readonly ITopicObjectService _topicObjectService;
        private readonly IBotSubscriptionObjectService _botSubscriptionObjectService;

        public PlanService(IPlanAreaService planAreaService,
            IPlanObjectService planObjectService,
            IPlanAreaObjectService planAreaObjectService,
            ITopicObjectService topicObjectService,
            IBotSubscriptionObjectService botSubscriptionObjectService)
        {
            _planAreaService = planAreaService;
            _planObjectService = planObjectService;
            _planAreaObjectService = planAreaObjectService;
            _topicObjectService = topicObjectService;
            _botSubscriptionObjectService = botSubscriptionObjectService;
        }

        public async Task<PlanResponseModel> CreatePlanAsync(PlanServiceModel model)
        {
            var plan = await CreatePlanCoreAsync(model);

            return new PlanResponseModel
            {
                Name = plan.Name,
                Id = plan.Id
            };
        }

        private async Task<Plan> CreatePlanCoreAsync(PlanServiceModel model)
        {
            var plan = new Plan(model.Name, model.UserId)
            {
                IsTemplate = model.IsTemplate
            };

            await _planObjectService.CreateAsync(plan);

            foreach (PlanAreaServiceModel planArea in model.PlanAreas)
            {
                PlanArea area = new PlanArea
                {
                    Name = planArea.Name,
                    Plan = plan,
                    PlanId = plan.Id,
                    UserId = plan.UserId
                };

                await _planAreaObjectService.CreateAsync(area);
                foreach (AreaTopicServiceModel areaTopic in planArea.AreaTopics)
                {
                    await _topicObjectService.CreateAsync(new AreaTopic
                    {
                        Name = areaTopic.Name,
                        StartDate = areaTopic.StartDate != null ? DateTime.ParseExact(areaTopic.StartDate, "yyyy-MM-dd",
                            CultureInfo.CurrentCulture) : (DateTime?) null,
                        EndDate = areaTopic.EndDate != null 
                            ? DateTime.ParseExact(areaTopic.EndDate, "yyyy-MM-dd", CultureInfo.CurrentCulture)
                            : (DateTime?) null,
                        Source = areaTopic.Source,
                        Description = areaTopic.Description,
                        PlanArea = area,
                        PlanAreaId = area.Id,
                        UserId = areaTopic.UserId,
                        PlanId = areaTopic.PlanId
                    });
                }
            }

            return plan;
        }

        public async Task DeleteAsync(string planId)
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

            foreach (BotSubscription botSubscription in _botSubscriptionObjectService.GetBotSubscriptionsByPlan(planId))
            {
                await _botSubscriptionObjectService.DeleteAsync(botSubscription);
            }

            await _planObjectService.DeleteAsync(plan);

        }

        public async Task<PlanServiceModel> GetByIdAsync(string id)
        {
            Plan plan = await _planObjectService.GetByIdAsync<Plan>(id);
            var planAreas = _planAreaObjectService.GetPlanAreas(plan.Id);
           
            return await Task.FromResult(new PlanServiceModel
            {
                Id = plan.Id,
                Name = plan.Name,
                UserId = plan.UserId,
                IsTemplate = plan.IsTemplate,
                PlanAreas = planAreas.Select(x => new PlanAreaServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PlanId = x.PlanId,
                    AreaTopics = _topicObjectService.GetTopicsByAreaId(x.Id).Select(areaTopic => new AreaTopicServiceModel
                    {
                        Id = areaTopic.Id,
                        Name = areaTopic.Name,
                        StartDate = areaTopic.StartDate?.ToString("yyyy-MM-dd"),
                        EndDate = areaTopic.EndDate?.ToString("yyyy-MM-dd"),
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
                    Name = plan.Name,
                    IsTemplate = plan.IsTemplate
                });
        }

        public IEnumerable<PlanResponseModel> GetAllTemplates()
        {
            return _planObjectService.GetTemplatePlans()
                .Select(plan => new PlanResponseModel
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    IsTemplate = plan.IsTemplate
                });
        }

        public async Task UpdateAsync(PlanServiceModel model)
        {

                Plan plan = await _planObjectService.GetByIdAsync<Plan>(model.Id);

                if (plan == null)
                {
                    throw new DomainServicesException("Plan not found.");
                }

                plan.Name = model.Name;

                await _planObjectService.UpdateAsync(plan);

        }

        public async Task CopyTemplatePlanAsync(string userId, string planId)
        {
           Plan plan = await _planObjectService.GetByIdAsync<Plan>(planId);
           var areas = _planAreaObjectService.GetPlanAreas(planId);

           await CreatePlanAsync(new PlanServiceModel
           {
               Name = plan.Name,
               IsTemplate = false,
               UserId = userId,
               PlanAreas = areas.Select(area => new PlanAreaServiceModel
               {
                   PlanId = area.PlanId,
                   Name = area.Name,
                   AreaTopics = _topicObjectService.GetTopicsByAreaId(area.Id)
                       .Select(topic => new AreaTopicServiceModel
                       {
                           IsTemplate = false,
                           Name = topic.Name,
                           StartDate = null,
                           EndDate = null,
                           Description = topic.Description,
                           Source = topic.Source,
                           UserId = userId,
                           PlanId = planId
                       }).ToArray()
               }).ToArray()
           });
        }
    }
}
