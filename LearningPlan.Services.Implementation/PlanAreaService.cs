using System;
using System.Globalization;
using System.Threading.Tasks;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using Microsoft.AspNetCore.Http;

namespace LearningPlan.Services.Implementation
{
    public class PlanAreaService : IPlanAreaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReadRepository<Plan> _planReadRepository;
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<PlanArea> _planAreaWriteRepository;

        public PlanAreaService(
            IHttpContextAccessor httpContextAccessor,
            IReadRepository<Plan> planReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            IWriteRepository<AreaTopic> areaTopicRepository, IWriteRepository<PlanArea> planAreaWriteRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _planReadRepository = planReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _areaTopicRepository = areaTopicRepository;
            _planAreaWriteRepository = planAreaWriteRepository;
        }

        public async Task<PlanAreaServiceModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model)
        {
            Plan plan = await _planReadRepository.GetByIdAsync(model.PlanId);

            ValidateUser(plan.UserId);

            PlanArea planArea = new PlanArea
            {
                Name = model.Name,
                PlanId = model.PlanId
            };
            await _planAreaWriteRepository.CreateAsync(planArea);

            await _planAreaWriteRepository.SaveChangesAsync();

            return new PlanAreaServiceModel
            {
                Id = planArea.Id,
                Name = planArea.Name,
                PlanId = planArea.PlanId,
                AreaTopics = Array.Empty<AreaTopicServiceModel>()
            };
        }

        public async Task<AreaTopicResponseModel> CreateAreaTopicAsync(CreateAreaTopicServiceModel model)
        {
            var planArea = await _planAreaReadRepository.GetByIdAsync(model.PlanAreaId);
            
            ValidateUser(planArea.Plan.UserId);

            AreaTopic areaTopic = new AreaTopic
            {
                PlanAreaId = model.PlanAreaId,
                PlanId =  planArea.PlanId,
                Name = model.Name,
                StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy",
                    CultureInfo.CurrentCulture),
                EndDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                Source = model.Source
            };
            await _areaTopicRepository.CreateAsync(areaTopic);
            await _areaTopicRepository.SaveChangesAsync();

            return new AreaTopicResponseModel
            {
                Id = areaTopic.Id,
                Name = areaTopic.Name,
                PlanId = areaTopic.PlanId
            };
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void ValidateUser(string userId)
        {
            if (userId != ((User)_httpContextAccessor.HttpContext.Items["User"]).Id)
            {
                throw new DomainServicesException("Plan not found.");
            }
        }

    }
}