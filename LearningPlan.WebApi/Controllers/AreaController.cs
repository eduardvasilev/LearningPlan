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

        /// <summary>
        /// Add new plan area
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddArea(CreatePlanAreaServiceModel model)
        {
            var plan = await  _planService.GetByIdAsync(model.PlanId);
            ValidateUser(plan.UserId);
            model.UserId = plan.UserId;
            var result = await _planAreaService.CreatePlanAreaAsync(model);
            return Json(result);
        }

        /// <summary>
        /// Delete plan area by id
        /// </summary>
        /// <param name="id">area id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var planArea = await _planAreaService.GetByIdAsync(id);
            ValidateUser(planArea.UserId);

            await _planAreaService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Edit plan area
        /// </summary>
        /// <param name="model">New plan area values</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task Update([FromBody] PlanAreaServiceModel model)
        {
            PlanArea planArea = await _planAreaService.GetByIdAsync(model.Id);
            ValidateUser(planArea.UserId);

            await _planAreaService.UpdateAsync(model);
        }
    }
}