using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;
using TrainingRecordSystemAPI.Services.Implementation;

namespace TrainingRecordSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _userService;

        public AdminController(IAdminService userService)
        {
            _userService = userService;
        }

        [HttpGet("MonthlyAdminReport")]
        public IActionResult MonthlyAdminReport(int userId, int? month, int? year)
        {
            var response = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>();

            response = _userService.MonthlyAdminReport(userId,month, year);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("DaterangeBasedReport")]
        public IActionResult DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate)
        {
            var response = new ServiceResponse<IEnumerable<DaterangeBasedReportDto>>();

            response = _userService.DaterangeBasedReport(jobId, startDate, endDate);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


      
        [HttpPost("AddTrainer")]
        public IActionResult AddTrainer(AddUserDto addUser)
        {
            var response = _userService.AddUser(addUser);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllTrainer")]
        public IActionResult GetAllTrainer()
        {
            var response = _userService.GetAllTrainer();
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetAllTrainerByPagination")]
        public IActionResult GetAllTraineryPagination(int page = 1, int pageSize = 6)
        {
            var response = _userService.GetAllTrainerByPagination(page, pageSize);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("TotalTrainerCount")]
        public IActionResult GetTotalCountOfPositions()
        {
            var response = _userService.TotalTrainer();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("AssignTopicToTrainer")]
        public IActionResult AssignTopicToTrainer(AssignTrainingTopicDto assignDto)
        {
            var response = _userService.AssignTopicToTrainer(assignDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllJobs")]
        public IActionResult GetAllJobs()
        {
            var response = _userService.getAllJobs();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetTrainerByLoginId/{id}")]
        public IActionResult GetTrainerByLoginId(string id)
        {
            var response = _userService.GetTrainerByLoginId(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("UnassignTopic")]
        public IActionResult UnassignTopic(int userId, int topicId)
        {
            var response = _userService.UnassignTopic(userId, topicId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
