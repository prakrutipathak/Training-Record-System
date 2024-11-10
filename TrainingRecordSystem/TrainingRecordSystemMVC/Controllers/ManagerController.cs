using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVC.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public ManagerController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }



        private IEnumerable<AllJobViewModel> GetAllJobs()
        {
            var apiUrl = $"{endPoint}Admin/GetAllJobs";

            ServiceResponse<IEnumerable<AllJobViewModel>> response = new ServiceResponse<IEnumerable<AllJobViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }


        //------------------UpComing Training Programs----------------
        [ManagerAuthorize]
        public IActionResult UpcomimngTrainingProgram(int jobId)
        {

            var apiUrl = "";

            if(jobId != 0)
            {
                apiUrl = $"{endPoint}Manager/UpcomingTrainingProgram?jobId="+ jobId ;
            }
            else
            {
                apiUrl = $"{endPoint}Manager/UpcomingTrainingProgram";

            }
                
                
            var response = _httpClientService.GetHttpResponseMessage<IEnumerable<UpcomingTrainingViewModel>>(apiUrl, HttpContext.Request);

            var jobs = GetAllJobs();

            ViewBag.Jobs = jobs;
            ViewBag.JobId = jobId;


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<UpcomingTrainingViewModel>>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "ErrorMessage";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<UpcomingTrainingViewModel>>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
        }

        //-------------------------Get Participants By Manager-----------------
        [HttpGet]
        [ManagerAuthorize]
        public IActionResult GetParticipantsByManagerId()
        {
            
            var id = User.FindFirst("UserId").Value;
            var apiUrl = $"{endPoint}Manager/GetParticipantByManagerId/" + id;
            var response = _httpClientService.GetHttpResponseMessage<IEnumerable<ParticipateViewModel>>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ParticipateViewModel>>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ParticipateViewModel>>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }

        }

        //---------------Add Participants By Manager-----------------
        [HttpGet]
        [ManagerAuthorize]
        public IActionResult AddParticipant()
        {
            IEnumerable<AllJobViewModel> jobs = GetAllJobs();
            ViewBag.Jobs = jobs;
            return View();

        }

        [HttpPost]
        [ManagerAuthorize]
        public IActionResult AddParticipant(AddParticipateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {


                var apiUrl = $"{endPoint}Manager/AddParticipate";
                var response = _httpClientService.PostHttpResponseMessage<AddParticipateViewModel>(apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddParticipateViewModel>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        return RedirectToAction("GetParticipantsByManagerId");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddParticipateViewModel>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                    return RedirectToAction("AddParticipant");
                }
            }

            IEnumerable<AllJobViewModel> jobs = GetAllJobs();
            ViewBag.Jobs = jobs;
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult NominatePartcipant(int participateId)
        {
            var managerDetailsApiUrl = $"{endPoint}Manager/GetParticipantById/" + participateId;

            ServiceResponse<ParticipateViewModel> response = new ServiceResponse<ParticipateViewModel>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<ParticipateViewModel>>
                (managerDetailsApiUrl, HttpMethod.Get, HttpContext.Request);

            if (response == null)
            {
                return RedirectToAction("GetParticipantsByManagerId");
            }

            var partcipateDetails = response.Data;

            ViewBag.PartcipateId = participateId;
            
            ViewBag.Topics = GetTrainerTopicsByJobId(partcipateDetails.JobId);
           
            if (response.Success)
            {
                return View(response.Data);
            }
            return RedirectToAction("GetParticipantsByManagerId");
        }


      
        private IEnumerable<TrainingProgramDetailJViewModel> GetTrainerTopicsByJobId(int jobId)
        {
            var apiUrl = $"{endPoint}Topic/GetTrainerTopicsByJobId"
                + "?jobId=" + jobId;
                    
            ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>> response = new ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingProgramDetailJViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);
           
            return response.Data;
        }

      
    }
}
