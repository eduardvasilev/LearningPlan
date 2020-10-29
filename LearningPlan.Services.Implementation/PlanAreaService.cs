using System;
using System.Globalization;
using System.Linq;
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
        private readonly ITopicService _topicService;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<PlanArea> _planAreaWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlanAreaService(
            IHttpContextAccessor httpContextAccessor,
            IReadRepository<Plan> planReadRepository,
            IReadRepository<PlanArea> planAreaReadRepository,
            ITopicService topicService,
            IWriteRepository<AreaTopic> areaTopicRepository, 
            IWriteRepository<PlanArea> planAreaWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _planReadRepository = planReadRepository;
            _planAreaReadRepository = planAreaReadRepository;
            _topicService = topicService;
            _areaTopicRepository = areaTopicRepository;
            _planAreaWriteRepository = planAreaWriteRepository;
            _unitOfWork = unitOfWork;
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
                StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture),
                EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd", CultureInfo.CurrentCulture),
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

        public async Task DeleteAsync(string id)
        {
            PlanArea planArea = await _planAreaReadRepository.GetByIdAsync(id);

            if (planArea == null)
            {
                throw new DomainServicesException("Plan Area not found.");
            }

            ValidateUser(planArea.Plan.UserId);

            foreach (AreaTopic areaTopic in _topicService.GetBy(planArea))
            {
                await _topicService.DeleteAsync(areaTopic.Id);
            }

            await _planAreaWriteRepository.DeleteAsync(planArea);
            await _planAreaWriteRepository.SaveChangesAsync();
        }

        public IQueryable<PlanArea> GetBy(Plan plan)
        {
            return _planAreaReadRepository.GetAll().Where(area => area.PlanId == plan.Id);
        }

        public async Task UpdateAsync(PlanAreaServiceModel model)
        {
            using (_unitOfWork)
            {
                PlanArea planArea = await _planAreaReadRepository.GetByIdAsync(model.Id);

                if (planArea == null)
                {
                    throw new DomainServicesException("Plan Area not found.");
                }

                ValidateUser(planArea.Plan.UserId);

                planArea.Name = model.Name;

                await _unitOfWork.CommitAsync();
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void ValidateUser(string userId)
        {
            if (userId != ((User)_httpContextAccessor.HttpContext.Items["User"]).Id)
            {
                throw new DomainServicesException("Current user has no permissions to do this action.");
            }
        }

    }
}