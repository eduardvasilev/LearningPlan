using System.Linq;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.Services;
using Quartz;
using Telegram.Bot;

namespace LearningPlan.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class TelegramNotificationJob : IJob
    {
        private readonly IBotSubscriptionService _botSubscriptionService;
        private readonly IBotService _botService;

        public TelegramNotificationJob(IBotSubscriptionService botSubscriptionService, IBotService botService)
        {
            _botSubscriptionService = botSubscriptionService;
            _botService = botService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (BotSubscriptionServiceModel botSubscription in _botSubscriptionService.GetAll())
            {
                var todayTopics = _botSubscriptionService.GetActualTopics(botSubscription.PlanId).ToList();

                if (todayTopics.Any())
                {
                    foreach (var topic in todayTopics)
                    {
                        await _botService.Client.SendTextMessageAsync(botSubscription.ChatId,
                            $"Your today's topic is {topic.Name}\r\nDue date: {topic.EndDate}.\r\nSource: {topic.Source}\r\nGood luck!");
                    }
                }
            }
        }
    }
}