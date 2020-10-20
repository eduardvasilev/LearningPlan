using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IPlanAreaService _planAreaService;

        public PlanController(IPlanService planService, IPlanAreaService planAreaService)
        {
            _planService = planService;
            _planAreaService = planAreaService;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PlanServiceModel model)
        {
            PlanResponseModel result = await _planService.CreatePlanAsync(model);
            return Json(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<PlanServiceModel> Get(string id)
        {
            return await _planService.GetByIdAsync(id);
        }

        [Authorize]
        [HttpGet]
        public List<PlanResponseModel> GetAll()
        {
            return _planService.GetAll().ToList();
        }
    }
}