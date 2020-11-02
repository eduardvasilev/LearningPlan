using LearningPlan.DomainModel;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AreaController : ControllerBase
    {

        private readonly IPlanAreaService _planAreaService;
        private readonly IPlanService _planService;

        public AreaController(IHttpContextAccessor httpContextAccessor,
            IPlanAreaService planAreaService, IPlanService planService) : base(httpContextAccessor)
        {
            _planAreaService = planAreaService;
            _planService = planService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddArea(CreatePlanAreaServiceModel model)
        {
            var plan = await  _planService.GetByIdAsync(model.PlanId);
            ValidateUser(plan.UserId);

            var result = await _planAreaService.CreatePlanAreaAsync(model);
            return Json(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var planArea = await _planAreaService.GetByIdAsync(id);
            ValidateUser(planArea.Plan.UserId);

            await _planAreaService.DeleteAsync(id);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task Update([FromBody] PlanAreaServiceModel model)
        {
            PlanArea planArea = await _planAreaService.GetByIdAsync(model.Id);
            ValidateUser(planArea.Plan.UserId);

            await _planAreaService.UpdateAsync(model);
        }
    }
}