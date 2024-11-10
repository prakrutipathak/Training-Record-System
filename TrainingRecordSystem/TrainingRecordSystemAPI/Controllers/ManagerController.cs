using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }


        [HttpGet("GetModeofTrainingByTopicId")]
        public IActionResult GetModeofTrainingByTopicId(int userId, int topicId)
        {
            var response = _managerService.GetModeofTrainingByTopicId(userId, topicId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPost("AddParticipate")]
        public IActionResult AddParticipate(AddParticipateDto participateDto)
        {
            if (ModelState.IsValid)
            {
                var participate = new Participate()
                {
                    FirstName = participateDto.FirstName,
                    LastName = participateDto.LastName,
                    Email = participateDto.Email,
                    UserId = participateDto.UserId,
                    JobId = participateDto.JobId,
                };
                var result = _managerService.AddParticipate(participate);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost("NominateParticipate")]
        public IActionResult NominateParticipate(NominateParticipateDto participateDto)
        {
            var response = _managerService.NominateParticipant(participateDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

     
         [HttpGet("UpcomingTrainingProgram")]
        public IActionResult UpcomingTrainingProgram(int? jobId)
        {
            var response = new ServiceResponse<IEnumerable<ManagerReport>>();
            response = _managerService.UpcomingTrainingProgram(jobId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetParticipantByManagerId/{id}")]
        public IActionResult GetParticipantByManagerId(int id)
        {
            var response = _managerService.GetParticipateByManageId(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetParticipantById/{id}")]
        public IActionResult GetParticipantById(int id)
        {
            var response = _managerService.GetParticipateById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
