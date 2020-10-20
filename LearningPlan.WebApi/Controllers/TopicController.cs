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

        public TopicController(IPlanAreaService planAreaService)
        {
            _planAreaService = planAreaService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTopic(CreateAreaTopicServiceModel model)
        {
            await _planAreaService.CreateAreaTopicAsync(model);
            return Ok();
        }
    }
}