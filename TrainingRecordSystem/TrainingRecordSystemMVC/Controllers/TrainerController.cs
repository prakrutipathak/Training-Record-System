using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using NuGet.Protocol;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVC.Controllers
{
    public class TrainerController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public TrainerController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }

        [TrainerAuthorize]

        public IActionResult Index(int page = 1, int pageSize = 4)
        {
          
            var trainerID = User.FindFirst("UserId").Value;
            var apiUrl = $"{endPoint}Trainer/GetAllTrainingTopicbyTrainerId/" + trainerID
                + "?page=" + page
                + "&pageSize=" + pageSize;

            var totalCountApiUrl = $"{endPoint}Trainer/TotalCountofTrainingTopicbyTrainerId/" + trainerID;

            ServiceResponse<int> countResponse = new ServiceResponse<int>();
            ServiceResponse<IEnumerable<TopicByPaginationViewModel>> response = new ServiceResponse<IEnumerable<TopicByPaginationViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicByPaginationViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

            var totalCount = countResponse.Data;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            if (response.Success)
            {

                return View(response.Data);
            }

            return View(new List<TopicByPaginationViewModel>());

        }


        [HttpGet]
        [TrainerAuthorize]
        public IActionResult AddProgramDetails()
        {
            return View();
        }
        [HttpPost]
        [TrainerAuthorize]
        public IActionResult AddProgramDetails(AddProgramDetailsViewModel addProgramDetailsViewModel)
        {
            var apiUrl = $"{endPoint}Trainer/AddTrainingProgramDetail";

            var response = _httpClientService.PostHttpResponseMessage<AddProgramDetailsViewModel>(apiUrl, addProgramDetailsViewModel, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddProgramDetailsViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddProgramDetailsViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong, please try after sometime.";
                }

                return View(addProgramDetailsViewModel);
            }
        }

        //------------------Get All Nominated Participants BY Pagination & Sorting--------------------
        [TrainerAuthorize]
        public IActionResult GetNominatedParticipants(int page = 1, int pageSize = 4, string sort_name = "default")
        {
            var apiUrl = $"{endPoint}Trainer/GetAllParticipantsByPaginationSorting"
                + "?page=" + page
                + "&pageSize=" + pageSize
                + "&sort_name=" + sort_name;


            var totalCountApiUrl = $"{endPoint}Trainer/TotalParticipantsCount";
                


            ServiceResponse<int> countResponse = new ServiceResponse<int>();
            ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>> response = new ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<GetNominatedParticipateViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

            var totalCount = countResponse.Data;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.sort_name = sort_name;
            

            if (response.Success)
            {

                return View(response.Data);
            }

            return View(new List<GetNominatedParticipateViewModel>());
        }

        //----Program detials----
        [TrainerAuthorize]
        public IActionResult GetProgramDetails(int id)
        {
            var trainerID = User.FindFirst("UserId").Value;
            var apiUrl = $"{endPoint}Trainer/GetAllTrainingProgramDetails?userId=" + trainerID + "&topicId=" + id;
            var response = _httpClientService.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<TrainingProgramDetailsViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<TrainingProgramDetailsViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime.";
                }
                return RedirectToAction("Index");
            }


        }

        [HttpGet]
        [TrainerAuthorize]
        public IActionResult UpdateProgramDetails(int topicId)
        {
         
            var trainerID = User.FindFirst("UserId").Value;
            var apiUrl = $"{endPoint}Trainer/GetAllTrainingProgramDetails?userId=" + trainerID + "&topicId=" + topicId;
            var response = _httpClientService.GetHttpResponseMessage<TrainingProgramDetailsViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<TrainingProgramDetailsViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    var details = serviceResponse.Data;
                    UpdateProgramDetailsViewModel viewModel = new UpdateProgramDetailsViewModel();
                    viewModel.TrainerProgramDetailId = details.TrainerProgramDetailId;
                    viewModel.StartDate = details.StartDate;
                    viewModel.EndDate = details.EndDate;
                    viewModel.StartTime = details.StartTime.Hour.ToString() + ":00";
                    viewModel.EndTime = details.EndTime.Hour.ToString() + ":00";
                    viewModel.ModePreference = details.ModePreference;
                    viewModel.TargetAudience = details.TargetAudience;
                    viewModel.TrainerTopicId = details.TrainerTopicId;
                    return View(viewModel);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<TrainingProgramDetailsViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [TrainerAuthorize]
        public IActionResult UpdateProgramDetails(UpdateProgramDetailsViewModel viewModel)
        {
            var apiUrl = $"{endPoint}Trainer/UpdateTrainingProgramDetail";

            var response = _httpClientService.PutHttpResponseMessage<UpdateProgramDetailsViewModel>(apiUrl, viewModel, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateProgramDetailsViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateProgramDetailsViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong, please try after sometime.";
                }

                return View(viewModel);
            }
        }
    }
}
