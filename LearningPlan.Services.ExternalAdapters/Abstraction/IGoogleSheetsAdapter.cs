using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningPlan.Services.ExternalAdapters.Models;
using LearningPlan.Services.Model;

namespace LearningPlan.Services.ExternalAdapters.Abstraction
{
    public interface IGoogleSheetsAdapter
    {
        Task<PlanResponseModel> ImportDataFromSheetAsync(ImportGoogleSheetModel model);
    }
}
