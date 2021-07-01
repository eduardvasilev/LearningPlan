using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Telegram.Bot;

namespace LearningPlan.TelegramBot.BackgroundService
{
    [DisallowConcurrentExecution]
    public class TelegramNotificationJob : IJob
    {
        private readonly IBotSubscriptionService _botSubscriptionService;
        private readonly IConfiguration _configuration;

        public TelegramNotificationJob(IBotSubscriptionService botSubscriptionService, IConfiguration configuration)
        {
            _botSubscriptionService = botSubscriptionService;
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string token = _configuration.GetSection("BotConfiguration:BotToken").Value;
            TelegramBotClient _client = new TelegramBotClient(token);
            foreach (BotSubscriptionServiceModel botSubscription in _botSubscriptionService.GetAll().ToList())
            {
                var todayTopics = _botSubscriptionService.GetActualTopics(botSubscription.PlanId).ToList();

                if (todayTopics.Any())
                {
                    foreach (var topic in todayTopics)
                    {
                        _client.SendTextMessageAsync(botSubscription.ChatId,
                            $"Your today's topic is {topic.Name}\r\nDue date: {topic.EndDate}.\r\nSource: {topic.Source}\r\nDescription: {topic.Description}\r\n Good luck!").GetAwaiter().GetResult();
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}