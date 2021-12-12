using LearningPlan.WebApi.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace LearningPlan.WebApi.Services
{
    public class BotService : IBotService
    {
        public BotService(IOptions<BotConfiguration> config)
        {
            Client = new TelegramBotClient(config.Value.BotToken);
        }

        public TelegramBotClient Client { get; }
    }
}