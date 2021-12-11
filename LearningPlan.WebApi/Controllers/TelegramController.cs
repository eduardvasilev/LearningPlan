using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LearningPlan.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TelegramController : Controller
    {
        private readonly IBotSubscriptionService _botSubscriptionService;
        private readonly IBotService _botService;
        private readonly IPlanService _planService;

        public TelegramController(IBotSubscriptionService botSubscriptionService, IBotService botService, 
            IPlanService planService)
        {
            _botSubscriptionService = botSubscriptionService;
            _botService = botService;
            _planService = planService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            var message = update.Message;
           
            if (message != null && message.Type == MessageType.Text)
            {
                if (message.Text != null && message.Text.StartsWith("/start"))
                {
                    Regex regex = new Regex(@"(?<=\/start\s)(.*)");
                    string planId = regex.Match(message.Text).Value;
                    if (string.IsNullOrEmpty(planId))
                    {
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                            "Please enter 'Plan Code' to subscribe it.");
                    }
                    else
                    {
                        await Subscribe(message, planId);
                    }
                }
                else
                {
                    await Subscribe(message, message.Text);
                }
            }


            return Ok();
        }

        private async Task Subscribe(Message message, string planId)
        {
            try
            {
                var plan = await _planService.GetByIdAsync(planId);
                if (plan == null)
                {
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Plan not found");
                    return;
                }

                PlanServiceModel result = await _botSubscriptionService.CreateBotSubscriptionAsync(
                    new BotSubscriptionServiceModel
                    {
                        ChatId = message.Chat.Id.ToString(),
                        PlanId = planId
                    });

                await _botService.Client.SendTextMessageAsync(message.Chat.Id,
                    $"You have successfully subscribed to '{result.Name}' plan.");
            }
            catch (DomainServicesException)
            {
                await _botService.Client.SendTextMessageAsync(message.Chat, "Oops... I don't feel good");
            }
        }
    }
}