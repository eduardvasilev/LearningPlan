using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using LearningPlan.Infrastructure;
using LearningPlan.Infrastructure.Implementation;
using LearningPlan.Infrastructure.Model;
using LearningPlan.ObjectServices;
using LearningPlan.ObjectServices.Implementation.Mongo;
using LearningPlan.Services;
using LearningPlan.Services.ExternalAdapters;
using LearningPlan.Services.ExternalAdapters.Abstraction;
using LearningPlan.Services.Implementation;
using LearningPlan.WebApi.Jobs;
using LearningPlan.WebApi.Middleware;
using LearningPlan.WebApi.Options;
using LearningPlan.WebApi.Services;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quartz;
using System;
using System.IO;
using System.Reflection;

namespace LearningPlan.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddHttpContextAccessor();

            services.Configure<EmailOptions>(
                Configuration.GetSection("EmailConfiguration"));

            services.Configure<FrontEndOptions>(
                Configuration.GetSection("FrontEndConfiguration"));

            services.AddScoped<ISmtpClient>(provider =>
            {
                var smtpClient = new SmtpClient();
                var emailOptions = provider.GetService<IOptions<EmailOptions>>()?.Value ?? throw new InvalidOperationException("Email sending isn't configured");
                smtpClient.Connect(emailOptions.SmtpServer, emailOptions.Port);
                smtpClient.Authenticate(emailOptions.UserName, emailOptions.Password);
                return smtpClient;
            });

            services.AddScoped(_ =>
            {
                string connectionString = Configuration["Database:ConnectionString"];
                MongoClient client = new MongoClient(connectionString);
                return client.GetDatabase(Configuration["Database:DatabaseName"]);
            });

            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserObjectService, UserObjectService>();
            services.AddScoped<ITopicObjectService, TopicObjectService>();
            services.AddScoped<IPlanObjectService, PlanObjectService>();
            services.AddScoped<IPlanAreaObjectService, PlanAreaObjectService>();
            services.AddScoped<IBotSubscriptionObjectService, BotSubscriptionObjectService>();
            services.AddScoped<IUserActivationCodeObjectService, UserActivationCodeObjectService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanAreaService, PlanAreaService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGoogleSheetsAdapter, GoogleSheetsAdapter>();
            services.AddScoped<IEmailSender, MailKitEmailSender>();

            services.AddSingleton<IBotService, BotService>();
            services.AddScoped<IBotSubscriptionService, BotSubscriptionService>();

            services.AddScoped(_ => new SheetsService(new BaseClientService.Initializer()
            {
                ApiKey = Configuration.GetSection("Google")["ApiKey"],
                ApplicationName = Configuration.GetSection("Google")["AppKey"],
            }));

   
            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey(nameof(TelegramNotificationJob));

                q.AddJob<TelegramNotificationJob>(opts => opts.WithIdentity(jobKey));

                // Create a trigger for the job
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{jobKey.Name}-trigger")
                    .WithSimpleSchedule(s => s.WithIntervalInHours(24).RepeatForever()));
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learning Plan Web API");
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
