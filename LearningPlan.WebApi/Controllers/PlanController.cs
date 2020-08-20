using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
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
        public async Task<IActionResult> Create(PlanServiceModel model)
        {
            await _planService.CreatePlanAsync(model);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<PlanServiceModel> Get(long id)
        {
            return await _planService.GetById(id);
        }
    }
}