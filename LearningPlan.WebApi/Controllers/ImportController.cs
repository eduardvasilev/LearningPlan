using LearningPlan.Services.ExternalAdapters.Abstraction;
using LearningPlan.Services.ExternalAdapters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearningPlan.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IGoogleSheetsAdapter _googleSheetsAdapter;

        public ImportController(IGoogleSheetsAdapter googleSheetsAdapter, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _googleSheetsAdapter = googleSheetsAdapter;
        }

        [HttpPost]
        [DataAnnotations.Authorize]
        [Route("googlesheets")]
        public async Task<IActionResult> GoogleSheets(ImportGoogleSheetModel model)
        {
            model.UserId = GetCurrentUser().Id;
            await _googleSheetsAdapter.ImportDataFromSheetAsync(model);
            return Ok();
        }
    }
}