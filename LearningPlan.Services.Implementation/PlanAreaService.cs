using System;
using System.Globalization;
using System.Threading.Tasks;
using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.Implementation
{
    public class PlanAreaService : IPlanAreaService
    {
        private readonly IWriteRepository<AreaTopic> _areaTopicRepository;
        private readonly IWriteRepository<PlanArea> _planAreaWriteRepository;

        public PlanAreaService(IWriteRepository<AreaTopic> areaTopicRepository, IWriteRepository<PlanArea> planAreaWriteRepository)
        {
            _areaTopicRepository = areaTopicRepository;
            _planAreaWriteRepository = planAreaWriteRepository;
        }

        public async Task<AreaTopicResponseModel> CreatePlanAreaAsync(CreatePlanAreaServiceModel model)
        {
            PlanArea planArea = new PlanArea
            {
                Name = model.Name,
                PlanId = model.PlanId
            };
            await _planAreaWriteRepository.CreateAsync(planArea);

            await _planAreaWriteRepository.SaveChangesAsync();

            return new AreaTopicResponseModel
            {
                Id = planArea.Id,
                Name = planArea.Name,
                PlanId = planArea.PlanId
            };
        }

        public async Task CreateAreaTopicAsync(CreateAreaTopicServiceModel model)
        {
            AreaTopic areaTopic = new AreaTopic
            {
                PlanAreaId = model.PlanAreaId,
                Name = model.Name,
                StartDate = DateTime.ParseExact(model.StartDate, "dd/mm/yyyy",
                    CultureInfo.CurrentCulture),
                EndDate = DateTime.ParseExact(model.EndDate, "dd/mm/yyyy", CultureInfo.CurrentCulture),
                Source = model.Source
            };
            await _areaTopicRepository.CreateAsync(areaTopic);
            await _areaTopicRepository.SaveChangesAsync();
        }
    }
}