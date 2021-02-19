﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class BotSubscriptionService : IBotSubscriptionService
    {
        private readonly IReadRepository<AreaTopic> _areaTopicReadRepository;
        private readonly IReadRepository<Plan> _planReadRepository;
        private readonly IReadRepository<BotSubscription> _botSubscriptionReadRepository;
        private readonly IWriteRepository<BotSubscription> _botSubscriptionWriteRepository;

        public BotSubscriptionService(
            IReadRepository<AreaTopic> areaTopicReadRepository,
            IReadRepository<Plan> planReadRepository,
            IReadRepository<BotSubscription> botSubscriptionReadRepository,
            IWriteRepository<BotSubscription> botSubscriptionWriteRepository)
        {
            _areaTopicReadRepository = areaTopicReadRepository;
            _planReadRepository = planReadRepository;
            _botSubscriptionReadRepository = botSubscriptionReadRepository;
            _botSubscriptionWriteRepository = botSubscriptionWriteRepository;
        }

        public async Task<PlanServiceModel> CreateBotSubscriptionAsync(BotSubscriptionServiceModel model)
        {
            Plan plan = await _planReadRepository.GetByIdAsync(model.PlanId);

            if (plan == null)
            {
                throw new DomainServicesException("Plan not found.");
            }

            if (await _botSubscriptionReadRepository
                .GetAll(x => x.PlanId == model.PlanId && x.ChatId == model.ChatId).CountAsync()> 0)
            {
                throw new DomainServicesException("You've already subscribed for this plan.");
            }

            await _botSubscriptionWriteRepository.CreateAsync(new BotSubscription
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

        public IAsyncEnumerable<BotSubscriptionServiceModel> GetAll()
        {
            return _botSubscriptionReadRepository.GetAll().Select(x => new BotSubscriptionServiceModel
            {
                PlanId = x.PlanId,
                ChatId = x.ChatId
            }).AsAsyncEnumerable();
        }

        public IAsyncEnumerable<AreaTopicServiceModel> GetActualTopics(string planId)
        {
            DateTime today = DateTime.Today;

            return _areaTopicReadRepository.GetAll(x => x.PlanId == planId)
                .Where(x =>
                    x.StartDate <= today && x.EndDate >= today)
                .Select(x => new AreaTopicServiceModel
                {
                    Name = x.Name,
                    StartDate = x.StartDate.ToString("d"),
                    EndDate = x.EndDate.ToString("d"),
                    Source = x.Source,
                }).AsAsyncEnumerable();
        }

    }
}
