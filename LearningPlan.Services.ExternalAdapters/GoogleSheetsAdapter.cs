using System;
using Google.Apis.Sheets.v4;
using LearningPlan.Services.ExternalAdapters.Abstraction;
using LearningPlan.Services.ExternalAdapters.Models;
using LearningPlan.Services.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.AspNetCore.Http;

namespace LearningPlan.Services.ExternalAdapters
{
    public class GoogleSheetsAdapter : IGoogleSheetsAdapter
    {
        private readonly SheetsService _sheetsService;
        private readonly IPlanService _planService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GoogleSheetsAdapter(SheetsService sheetsService, IPlanService planService, IHttpContextAccessor httpContextAccessor)
        {
            _sheetsService = sheetsService;
            _planService = planService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PlanResponseModel> ImportDataFromSheetAsync(ImportGoogleSheetModel model)
        {
            string userId = ((User) _httpContextAccessor.HttpContext.Items["User"])?.Id;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not found");
            }
            PlanServiceModel planServiceModel = new PlanServiceModel
            {
                IsTemplate = model.IsTemplate,
                UserId = userId,
                Name = model.PlanName,
            };
            
            var sheet = await _sheetsService.Spreadsheets.Values.Get(model.SheetId, model.Range).ExecuteAsync();
            var areas = new Dictionary<string, List<AreaTopicServiceModel>>();
            string previousKey = string.Empty;
            foreach (var sheetValue in sheet.Values.ToList())
            {
                int sheetValueCount = sheetValue.Count - 1;
                if (model.AreaColumn > sheetValueCount ||
                    model.TopicColumn > sheetValueCount)
                {
                    throw new DomainServicesException("Invalid column.");
                }

                string area = sheetValue[model.AreaColumn].ToString();
                if (!string.IsNullOrEmpty(area))
                {
                    previousKey = area;
                }
                string key = string.IsNullOrWhiteSpace(area) ? previousKey : area;
                if (areas.ContainsKey(key))
                {
                    areas[key].Add(new AreaTopicServiceModel
                    {
                        Name = sheetValue[model.TopicColumn].ToString(),
                        Description = model.DescriptionColumn.HasValue && model.DescriptionColumn < sheetValueCount ? sheetValue[model.DescriptionColumn.Value].ToString() : null,
                        StartDate = model.StartDateColumn.HasValue && model.StartDateColumn < sheetValueCount ? sheetValue[model.StartDateColumn.Value].ToString() : null,
                        EndDate = model.EndDateColumn.HasValue && model.EndDateColumn < sheetValueCount ? sheetValue[model.EndDateColumn.Value].ToString() : null,
                        Source = model.SourceColumn.HasValue && model.SourceColumn < sheetValueCount ? sheetValue[model.SourceColumn.Value].ToString() : null,
                        UserId = userId

                    });
                }
                else
                {
                    areas.Add(key, new List<AreaTopicServiceModel>
                    {
                        new AreaTopicServiceModel
                        {
                            Name = sheetValue[model.TopicColumn].ToString(),
                            Description = model.DescriptionColumn.HasValue && model.DescriptionColumn < sheetValueCount ? sheetValue[model.DescriptionColumn.Value].ToString() : null,
                            StartDate = model.StartDateColumn.HasValue && model.StartDateColumn < sheetValueCount ? sheetValue[model.StartDateColumn.Value].ToString() : null,
                            EndDate = model.EndDateColumn.HasValue && model.EndDateColumn < sheetValueCount ? sheetValue[model.EndDateColumn.Value].ToString() : null,
                            Source = model.SourceColumn.HasValue && model.SourceColumn < sheetValueCount ? sheetValue[model.SourceColumn.Value].ToString() : null,
                            UserId = userId

                        }
                    });
                }
            }

            planServiceModel.PlanAreas = areas.Select(x => new PlanAreaServiceModel
            {
                Name = x.Key,
                AreaTopics = x.Value.ToArray()
            }).ToArray();

            return await _planService.CreatePlanAsync(planServiceModel);
        }
    }
}
