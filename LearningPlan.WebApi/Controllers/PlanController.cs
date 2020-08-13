using System.Threading.Tasks;
using LearningPlan.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery]string name)
        {
            await _planService.CreatePlanAsync(name);
            return Json(name);
        }
    }
}