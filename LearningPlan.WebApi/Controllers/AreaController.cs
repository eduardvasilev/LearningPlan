using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AreaController : Controller
    {
        private readonly IPlanAreaService _planAreaService;

        public AreaController(IPlanAreaService planAreaService)
        {
            _planAreaService = planAreaService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddArea(CreatePlanAreaServiceModel model)
        {
            var result = await _planAreaService.CreatePlanAreaAsync(model);
            return Json(result);
        }
    }
}