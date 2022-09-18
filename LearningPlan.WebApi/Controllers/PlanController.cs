using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IHttpContextAccessor httpContextAccessor, IPlanService planService) : base(httpContextAccessor)
        {
            _planService = planService;
        }
        
        /// <summary>
        /// Create new learning plan.
        /// </summary>
        /// <param name="model">Description of creating plan.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PlanServiceModel model)
        {
            model.UserId = GetCurrentUser().Id;
            PlanResponseModel result = await _planService.CreatePlanAsync(model);
            return Json(result);
        }

        /// <summary>
        /// Get plan by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<PlanServiceModel> Get(string id)
        {
            return await _planService.GetByIdAsync(id);
        }

        /// <summary>
        /// Get all plans
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<List<PlanResponseModel>> GetAll()
        {
            var user = GetCurrentUser();

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return (await _planService.GetAll(user.Id)).ToList();
        }

        /// <summary>
        /// Get all plans of user
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<List<PlanResponseModel>> GetAllTemplatesByUser(string userId)
        {
            return (await _planService.GetAll(userId, true)).ToList();
        }

        /// <summary>
        /// Get all template plans
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("templates")]
        public List<PlanResponseModel> GetAllTemplates()
        {
            return _planService.GetAllTemplates().ToList();
        }

        /// <summary>
        /// Copies template plan to user's account
        /// </summary>
        /// <param name="planId">Id of plan</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("copy/{planId}")]
        public async Task<IActionResult> CopyTemplatePlan(string planId)
        {
             await _planService.CopyTemplatePlanAsync(GetCurrentUser().Id, planId);
             return Ok();
        }

        /// <summary>
        /// Copies user's plan as template plan
        /// </summary>
        /// <param name="planId">Id of plan</param>
        /// <returns></returns>
        [HttpPost("{planId}/template")]
        public async Task<IActionResult> SaveAsTemplate(string planId)
        {
            await _planService.SaveAsTemplateAsync(GetCurrentUser().Id, planId);
            return Ok();
        }

        /// <summary>
        /// Delete plan by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var plan = await _planService.GetByIdAsync(id);
            ValidateUser(plan.UserId);

            await _planService.DeleteAsync(id);
        }

        /// <summary>
        /// Edit plan
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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