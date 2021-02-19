using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.Services.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation
{
    public class PlanAreaService : IPlanAreaService
    {
        private readonly IReadRepository<PlanArea> _planAreaReadRepository;
        private readonly ITopicService _topicService;
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<PlanArea> _planAreaWriteRepository;
        private readonly IReadRepository<Plan> _planReadRepository;

        public PlanAreaService(IReadRepository<PlanArea> planAreaReadRepository,
            ITopicService topicService,
            IWriteRepository<AreaTopic> areaTopicRepository, 
            IWriteRepository<PlanArea> planAreaWriteRepository,
            IReadRepository<Plan> planReadRepository)
        {
            _planAreaReadRepository = planAreaReadRepository;
            _topicService = topicService;
            _areaTopicRepository = areaTopicRepository;
            _planAreaWriteRepository = planAreaWriteRepository;
            _planReadRepository = planReadRepository;
        }

        public async Task<PlanAreaServiceModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model)
        {
            var plan = await _planReadRepository.GetByIdAsync(model.PlanId);
            PlanArea planArea = new PlanArea
            {
                Name = model.Name,
                PlanId = model.PlanId,
                UserId = plan.UserId
            };
            await _planAreaWriteRepository.CreateAsync(planArea);

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
            
            AreaTopic areaTopic = new AreaTopic
            {
                PlanAreaId = model.PlanAreaId,
                PlanId =  planArea.PlanId,
                Name = model.Name,
                StartDate = DateTime.ParseExact(model.StartDate, "yyyy-MM-dd",
                    CultureInfo.CurrentCulture),
                EndDate = DateTime.ParseExact(model.EndDate, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                Source = model.Source,
                Description = model.Description,
                UserId = planArea.UserId
            };
            await _areaTopicRepository.CreateAsync(areaTopic);

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

            foreach (AreaTopic areaTopic in _topicService.GetBy(planArea))
            {
                await _topicService.DeleteAsync(areaTopic.Id);
            }

            await _planAreaWriteRepository.DeleteAsync(planArea);
        }

        public async Task<PlanArea> GetByIdAsync(string id)
        {
            return await _planAreaReadRepository.GetByIdAsync(id);
        }

        public IEnumerable<PlanArea> GetBy(Plan plan)
        {
            return _planAreaReadRepository.GetAll(area => area.PlanId == plan.Id);
        }

        public async Task UpdateAsync(PlanAreaServiceModel model)
        {
           
            PlanArea planArea = await _planAreaReadRepository.GetByIdAsync(model.Id);

            if (planArea == null)
            {
                throw new DomainServicesException("Plan Area not found.");
            }
            
            planArea.Name = model.Name;

            await _planAreaWriteRepository.UpdateAsync(planArea);
        }
    }
}