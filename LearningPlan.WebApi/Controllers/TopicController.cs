using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LearningPlan.Services;
using LearningPlan.Services.Model;
using LearningPlan.WebApi.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlan.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : Controller
    {
        private readonly IPlanAreaService _planAreaService;
        private readonly ITopicService _topicService;

        public TopicController(IPlanAreaService planAreaService, ITopicService topicService)
        {
            _planAreaService = planAreaService;
            _topicService = topicService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTopic(CreateAreaTopicServiceModel model)
        {
            AreaTopicResponseModel response = await _planAreaService.CreateAreaTopicAsync(model);
            return Json(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([Required]string id)
        {
            await _topicService.DeleteAsync(id);
            return Ok();
        }
    }
}