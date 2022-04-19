using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using LearningPlan.ObjectServices;

namespace LearningPlan.Services.Implementation
{
    public class PlanAreaService : IPlanAreaService
    {
        private readonly ITopicService _topicService;
        private readonly IPlanAreaObjectService _planAreaObjectService;
        private readonly ITopicObjectService _topicObjectService;
        private readonly IBotSubscriptionObjectService _botSubscriptionObjectService;

        public PlanAreaService(
            ITopicService topicService,
            IPlanAreaObjectService planAreaObjectService, 
            ITopicObjectService topicObjectService,
            IBotSubscriptionObjectService botSubscriptionObjectService)
        {
            _topicService = topicService;
            _planAreaObjectService = planAreaObjectService;
            _topicObjectService = topicObjectService;
            _botSubscriptionObjectService = botSubscriptionObjectService;
        }

        public async Task<PlanAreaServiceModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model)
        {
            PlanArea planArea = new PlanArea
            {
                Name = model.Name,
                PlanId = model.PlanId,
                UserId = model.UserId
            };
            await _planAreaObjectService.CreateAsync(planArea);

            return new PlanAreaServiceModel
            {
                Id = planArea.Id,
                Name = planArea.Name,
                PlanId = planArea.PlanId,
                AreaTopics = Array.Empty<AreaTopicServiceModel>()
            };
        }

        public async Task<AreaTopicResponseModel> CreateAreaTopicAsync(CreateAreaTopicServiceModel model)
        {
            var planArea = await _planAreaObjectService.GetByIdAsync<PlanArea>(model.PlanAreaId);
            
            AreaTopic areaTopic = new AreaTopic
            {
                PlanAreaId = model.PlanAreaId,
                PlanId =  planArea.PlanId,
                UserId =  planArea.UserId,
                Name = model.Name,
                Source = model.Source,
                Description = model.Description
            };

            if (!model.IsTemplate && !string.IsNullOrEmpty(model.StartDate) && !string.IsNullOrEmpty(model.EndDate))
            {
                areaTopic.StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture);
                areaTopic.EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            }
            await _topicObjectService.CreateAsync(areaTopic);

            return new AreaTopicResponseModel
            {
                Id = areaTopic.Id,
                Name = areaTopic.Name,
                PlanId = areaTopic.PlanId
            };
        }

        public async Task DeleteAsync(string id)
        {
            PlanArea planArea = await _planAreaObjectService.GetByIdAsync<PlanArea>(id);

            if (planArea == null)
            {
                throw new DomainServicesException("Plan Area not found.");
            }

            foreach (AreaTopic areaTopic in _topicService.GetBy(planArea))
            {
                await _topicService.DeleteAsync(areaTopic.Id);
            }



            await _planAreaObjectService.DeleteAsync(planArea);
        }

        public async Task<PlanArea> GetByIdAsync(string id)
        {
            return await _planAreaObjectService.GetByIdAsync<PlanArea>(id);
        }

        public IEnumerable<PlanArea> GetBy(Plan plan)
        {
            return _planAreaObjectService.GetPlanAreas(plan.Id);
        }

        public async Task UpdateAsync(PlanAreaServiceModel model)
        {
            PlanArea planArea = await _planAreaObjectService.GetByIdAsync<PlanArea>(model.Id);

            if (planArea == null)
            {
                throw new DomainServicesException("Plan Area not found.");
            }

            planArea.Name = model.Name;

            await _planAreaObjectService.UpdateAsync(planArea);
        }
    }
}