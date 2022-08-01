using LearningPlan.DomainModel.Exceptions;
using LearningPlan.ObjectServices;
using LearningPlan.ObjectServices.Implementation.Mongo;
using LearningPlan.Services;
using LearningPlan.Services.Implementation;
using LearningPlan.WebApi.Middleware;
using LearningPlan.WebApi.Options;
using LearningPlan.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.IO;
using System.Reflection;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using LearningPlan.Services.ExternalAdapters;
using LearningPlan.Services.ExternalAdapters.Abstraction;
using LearningPlan.WebApi.Jobs;
using Quartz;

namespace LearningPlan.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            //services.AddDbContext<EfContext>(options =>
            //    options.UseLazyLoadingProxies().
            //        UseCosmos(
            //            Configuration["Database:AccountEndpoint"],
            //            Configuration["Database:AccountKey"],
            //            databaseName: Configuration["Database:DatabaseName"]));

            services.AddHttpContextAccessor();

            services.AddScoped(provider =>
            {
                string connectionString = Configuration["Database:ConnectionString"];
                MongoClient client = new MongoClient(connectionString);
                return client.GetDatabase(Configuration["Database:DatabaseName"]);
            });
            //services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            //services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserObjectService, UserObjectService>();
            services.AddScoped<ITopicObjectService, TopicObjectService>();
            services.AddScoped<IPlanObjectService, PlanObjectService>();
            services.AddScoped<IPlanAreaObjectService, PlanAreaObjectService>();
            services.AddScoped<IBotSubscriptionObjectService, BotSubscriptionObjectService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanAreaService, PlanAreaService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGoogleSheetsAdapter, GoogleSheetsAdapter>();

            services.AddSingleton<IBotService, BotService>();
            services.AddScoped<IBotSubscriptionService, BotSubscriptionService>();

            services.AddScoped(provider => new SheetsService(new BaseClientService.Initializer()
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is DomainServicesException)
                        {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "application/json";
                            var response = new { message = exceptionHandlerPathFeature.Error.Message };
                            await context.Response.WriteAsJsonAsync(response);
                        }
                    });
                });
            }

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
