using Telegram.Bot;

namespace LearningPlan.WebApi.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}