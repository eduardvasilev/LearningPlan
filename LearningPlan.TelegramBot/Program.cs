using System;
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
using Telegram.Bot.Types.Enums;

namespace LearningPlan.TelegramBot
{
    class Program
    {
        private static TelegramBotClient client;
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
            client = new TelegramBotClient(token);
            client.OnMessage += BotOnMessageReceived;
            client.OnMessageEdited += BotOnMessageReceived;
            client.StartReceiving();

            Console.ReadLine();

            client.StopReceiving();
            DisposeServices();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message?.Type == MessageType.Text)
            {
                if (message.Text == "/start")
                {
                    await client.SendTextMessageAsync(message.Chat.Id, "Please enter 'Plan Code' to subscribe it.");
                }
                else
                {
                    IServiceScope scope = _serviceProvider.CreateScope();
                    IBotSubscriptionService botSubscriptionService = (IBotSubscriptionService)scope.ServiceProvider.GetService(typeof(IBotSubscriptionService));

                    try
                    {
                        PlanServiceModel result = await botSubscriptionService.CreateBotSubscriptionAsync(new BotSubscriptionServiceServiceModel
                        {
                            ChatId = message.Chat.Id.ToString(),
                            PlanId = message.Text
                        });

                        await client.SendTextMessageAsync(message.Chat.Id, $"You have successfully subscribed to '{result.Name}' plan.");

                    }
                    catch (DomainServicesException exception)
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, exception.Message);
                    }
                }
            }
        }

        private static void RegisterServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddDbContext<EfContext>(options =>
                options.UseCosmos(
                        configuration["Database:AccountEndpoint"],
                        configuration["Database:AccountKey"],
                        databaseName: configuration["Database:DatabaseName"]));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
