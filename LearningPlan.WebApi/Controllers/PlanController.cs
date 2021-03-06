﻿using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPlan.WebApi.Controllers
{
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
        public List<PlanResponseModel> GetAll()
        {
            return _planService.GetAll(GetCurrentUser()).ToList();
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