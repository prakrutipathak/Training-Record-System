using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet("GetTopicsByJobId")]
        public IActionResult GetTopicsByJobId(int jobId)
        {
            var response = _topicService.GetTopicsByJobId(jobId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetTrainerTopicsByJobId")]
        public IActionResult GetTrainerTopicsByJobId(int jobId)
        {
            var response = _topicService.GetTrainerTopicsByJobId(jobId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetTrainerByTopicId")]
        public IActionResult GetTrainerByTopicId(int topicId)
        {
            var response = _topicService.GetTrainerByTopicId(topicId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
