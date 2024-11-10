using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {

        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

  

        [HttpGet("GetAllTrainingTopicbyTrainerId/{trainerId}")]
        public IActionResult GetAllTrainingTopicbyTrainerId(int trainerId, int page = 1, int pageSize = 6)
        {
            var response = _trainerService.GetAllTrainingTopicbyTrainerId(trainerId,page,pageSize);
            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpGet("TotalCountofTrainingTopicbyTrainerId/{trainerId}")]
        public IActionResult TotalCountofTrainingTopicbyTrainerId(int trainerId)
         {
            var response = _trainerService.TotalCountofTrainingTopicbyTrainerId(trainerId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //-----------Get All Participants -----
        [HttpGet("GetAllParticipantsByPaginationSorting")]
        public IActionResult GetAllTraineryPagination(int page = 1, int pageSize = 6, string sort_name = "default")
        {
            var response = _trainerService.GetAllParticipantsByPAgination(page, pageSize, sort_name);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("TotalParticipantsCount")]
        public IActionResult GetTotalCountOfPositions()
        {
            var response = _trainerService.TotalParticipants();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPost("AddTrainingProgramDetail")]
        public IActionResult AddTrainingProgramDetail(AddTrainingProgramDetailDto trainingProgramDetailDto)
        {
            var response = _trainerService.AddTrainingProgramDetail(trainingProgramDetailDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllTrainingProgramDetails")]
        public IActionResult GetAllTrainingProgrambyTrainerId(int userId, int topicId)
        {
            var response = _trainerService.GetAllTraniningProgramDetails(userId, topicId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpPut("UpdateTrainingProgramDetail")]
        public IActionResult UpdateTrainingProgramDetail(UpdateTrainingProgramDetailDto trainingProgramDetailDto)
        {
            var response = _trainerService.UpdateTrainingProgramDetails(trainingProgramDetailDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
