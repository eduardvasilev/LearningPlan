using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LearningPlan.DataAccess;
using LearningPlan.DataAccess.Implementation;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services;
using LearningPlan.Services.Implementation;
using LearningPlan.Services.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LearningPlan.TelegramBot
{
    class Program
    {
        private static TelegramBotClient _client;
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            RegisterServices(configuration);

            string token = configuration.GetSection("BotConfiguration:BotToken").Value;
            _client = new TelegramBotClient(token);
            _client.OnMessage += BotOnMessageReceived;
            _client.OnMessageEdited += BotOnMessageReceived;
            _client.StartReceiving();

            Console.ReadLine();

            _client.StopReceiving();
            DisposeServices();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message?.Type == MessageType.Text)
            {
                if (message.Text.StartsWith("/start"))
                {
                    Regex regex = new Regex(@"(?<=\/start\s)(.*)");
                    string planId = regex.Match(message.Text).Value;
                    if (string.IsNullOrEmpty(planId))
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Please enter 'Plan Code' to subscribe it.");
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
        }

        private static async Task Subscribe(Message message, string planId)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IBotSubscriptionService botSubscriptionService =
                (IBotSubscriptionService) scope.ServiceProvider.GetService(typeof(IBotSubscriptionService));

            try
            {
                PlanServiceModel result = await botSubscriptionService.CreateBotSubscriptionAsync(
                    new BotSubscriptionServiceModel
                    {
                        ChatId = message.Chat.Id.ToString(),
                        PlanId = planId
                    });

                await _client.SendTextMessageAsync(message.Chat.Id,
                    $"You have successfully subscribed to '{result.Name}' plan.");
            }
            catch (DomainServicesException exception)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, exception.Message);
            }
        }

        private static void RegisterServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
       
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IBotSubscriptionService, BotSubscriptionService>();
            services.AddScoped<IPlanAreaService, PlanAreaService>();
            services.AddScoped<IUserService, UserService>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
