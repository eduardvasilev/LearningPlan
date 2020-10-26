using LearningPlan.DataAccess;
using LearningPlan.DataAccess.Implementation;
using LearningPlan.Services;
using LearningPlan.Services.Implementation;
using LearningPlan.Services.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Telegram.Bot;

namespace LearningPlan.TelegramBot.BackgroundService
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
            Process();

            DisposeServices();
        }

        private static void Process()
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IBotSubscriptionService botSubscriptionService =
                (IBotSubscriptionService)scope.ServiceProvider.GetService(typeof(IBotSubscriptionService));
            ITopicService topicService = (ITopicService)scope.ServiceProvider.GetService(typeof(ITopicService));
            foreach (BotSubscriptionServiceModel botSubscription in botSubscriptionService.GetAll())
            {
                var todayTopics = topicService.GetActualTopics(botSubscription.PlanId).ToList();

                if (todayTopics.Any())
                {
                    foreach (var topic in todayTopics)
                    {
                        _client.SendTextMessageAsync(botSubscription.ChatId, 
                            $"Your today's topic is {topic.Name}\r\nDue date: {topic.EndDate}.\r\nSource: {topic.Source}\r\nGood luck!").GetAwaiter().GetResult();
                    }
                }
                else
                {
                    //TODO add plan.IsFinished
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IBotSubscriptionService, BotSubscriptionService>();
            services.AddScoped<IPlanAreaService, PlanAreaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITopicService, TopicService>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
