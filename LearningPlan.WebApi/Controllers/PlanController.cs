using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningPlan.DomainModel;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPlanService _planService;

        public PlanController(IHttpContextAccessor httpContextAccessor, IPlanService planService) : base(httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _planService = planService;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PlanServiceModel model)
        {
            model.UserId = GetCurrentUser().Id;
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
            return _planService.GetAll(GetCurrentUser()).ToList();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var plan = await _planService.GetByIdAsync(id);
            ValidateUser(plan.UserId);

            await _planService.DeleteAsync(id);
        }

        [Authorize]
        [HttpPut]
        public async Task Update([FromBody]PlanServiceModel model)
        {
            var plan = await _planService.GetByIdAsync(model.Id);
            ValidateUser(plan.UserId);

            await _planService.UpdateAsync(model);
        }
    }
}