using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.ObjectServices;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class BotSubscriptionService : IBotSubscriptionService
    {
        private readonly IPlanObjectService _planObjectService;
        private readonly IBotSubscriptionObjectService _botSubscriptionObjectService;
        private readonly ITopicObjectService _topicObjectService;

        public BotSubscriptionService(IPlanObjectService planObjectService,
            IBotSubscriptionObjectService botSubscriptionObjectService,
            ITopicObjectService topicObjectService)
        {
            _planObjectService = planObjectService;
            _botSubscriptionObjectService = botSubscriptionObjectService;
            _topicObjectService = topicObjectService;
        }

        public async Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceModel model)
        {
            Plan plan = await _planObjectService.GetByIdAsync<Plan>(model.PlanId);

            if (plan == null)
            {
                throw new DomainServicesException("Plan not found.");
            }

            if (_botSubscriptionObjectService.GetBotSubscriptionsByPlanAndChat(model.PlanId, model.ChatId).Any())
            {
                throw new DomainServicesException("You've already subscribed for this plan.");
            }

            await _botSubscriptionObjectService.CreateAsync(new BotSubscription
            {
                ChatId = model.ChatId,
                PlanId = model.PlanId
            });

            return new PlanServiceModel
            {
                Id = plan.Id,
                Name = plan.Name
            };
        }

        public IEnumerable<BotSubscriptionServiceModel> GetAll()
        {
            return _botSubscriptionObjectService.GetAll().Select(x => new BotSubscriptionServiceModel
            {
                PlanId = x.PlanId,
                ChatId = x.ChatId
            });
        }

        public IEnumerable<AreaTopicServiceModel> GetActualTopics(string planId)
        {
            DateTime today = DateTime.Today;

            return _topicObjectService.GetTopicsByPlanForToday(planId, today)
                .Select(x => new AreaTopicServiceModel
                {
                    Name = x.Name,
                    StartDate = x.StartDate.ToString("d"),
                    EndDate = x.EndDate.ToString("d"),
                    Source = x.Source,
                })
                .ToList();
        }

    }
}
