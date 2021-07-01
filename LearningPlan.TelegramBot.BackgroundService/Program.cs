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
using Microsoft.Extensions.Hosting;
using Quartz;
using Telegram.Bot;

namespace LearningPlan.TelegramBot.BackgroundService
{
    class Program
    {
        static void Main(string[] args) => CreateHostBuilder(args).Build().Run();
  
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .Build();

                    services.AddDbContext<EfContext>(options =>
                        options.
                            UseNpgsql(configuration.GetSection("Database:ConnectionString").Value));

                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
                    services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddScoped<IPlanService, PlanService>();
                    services.AddScoped<IBotSubscriptionService, BotSubscriptionService>();
                    services.AddScoped<IPlanAreaService, PlanAreaService>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<ITopicService, TopicService>();

                    services.AddQuartz(q =>
                    {
                        var jobKey = new JobKey(nameof(TelegramNotificationJob));

                        q.AddJob<TelegramNotificationJob>(opts => opts.WithIdentity(jobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity($"{jobKey.Name}-trigger")
                            .WithCronSchedule("0 12 * * * ?")); //todo configure for each user
                        q.UseMicrosoftDependencyInjectionJobFactory();
                    });

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);

                });

    }
}
