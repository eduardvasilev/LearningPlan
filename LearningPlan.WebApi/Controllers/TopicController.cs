using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly IPlanAreaService _planAreaService;
        private readonly ITopicService _topicService;

        public TopicController(IHttpContextAccessor httpContextAccessor, IPlanAreaService planAreaService, ITopicService topicService)
        : base(httpContextAccessor)
        {
            _planAreaService = planAreaService;
            _topicService = topicService;
        }

        /// <summary>
        /// Add new plan area topic
        /// </summary>
        /// <param name="model">Description of topic</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTopic(CreateAreaTopicServiceModel model)
        {
            var planArea = await _planAreaService.GetByIdAsync(model.PlanAreaId);
            ValidateUser(planArea.UserId);

            AreaTopicResponseModel response = await _planAreaService.CreateAreaTopicAsync(model);
            return Json(response);
        }

        /// <summary>
        /// Delete topic by id
        /// </summary>
        /// <param name="id">topic id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([Required]string id)
        {
            var topic = await _topicService.GetByIdAsync(id);
            ValidateUser(topic.UserId);

            await _topicService.DeleteAsync(id);
            return Ok();
        }

        /// <summary>
        /// Edit topic
        /// </summary>
        /// <param name="model">New values of topic</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task Update(AreaTopicServiceModel model)
        {
            var topic = await _topicService.GetByIdAsync(model.Id);
            ValidateUser(topic.UserId);

            await _topicService.UpdateAsync(model);
        }
    }
}