using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace TrainingRecordSystemMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public AdminController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }


        [AdminAuthorize]
        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            var apiUrl = $"{endPoint}Admin/GetAllTrainerByPagination"
                + "?page=" + page
                + "&pageSize=" + pageSize;

            var totalCountApiUrl = $"{endPoint}Admin/TotalTrainerCount";

            ServiceResponse<int> countResponse = new ServiceResponse<int>();
            ServiceResponse<IEnumerable<TrainerByPaginationViewModel>> response = new ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainerByPaginationViewModel>>>
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

            return View(new List<TrainerByPaginationViewModel>());
        }


        private IEnumerable<AllJobViewModel> GetAllJobs()
        {
            var apiUrl = $"{endPoint}Admin/GetAllJobs";

            ServiceResponse<IEnumerable<AllJobViewModel>> response = new ServiceResponse<IEnumerable<AllJobViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<AllJobViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        private List<AllTrainersViewModel> GetAllTrainers()
        {
            var apiUrl = $"{endPoint}Admin/GetAllTrainer";

            ServiceResponse<IEnumerable<AllTrainersViewModel>> response = new ServiceResponse<IEnumerable<AllTrainersViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<AllTrainersViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }

            return new List<AllTrainersViewModel>();

        }

        [HttpGet]
        [AdminAuthorize]
        public IActionResult Create()
        {
            IEnumerable<AllJobViewModel> jobs = GetAllJobs();
            ViewBag.Jobs = jobs;
            return View();

        }

        [HttpPost]
        [AdminAuthorize]
        public IActionResult Create(AddTrainerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {


                var apiUrl = $"{endPoint}Admin/AddTrainer";
                var response = _httpClientService.PostHttpResponseMessage<AddTrainerViewModel>(apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddTrainerViewModel>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = serviceResponse?.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddTrainerViewModel>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                    return RedirectToAction("Create");
                }
            }

            IEnumerable<AllJobViewModel> jobs = GetAllJobs();
            ViewBag.Jobs = jobs;
            return View(viewModel);
        }


        [HttpGet]
        [AdminAuthorize]
        public IActionResult AssignTopic(int userId)
        {
            var userDetailsApiUrl = $"{endPoint}Auth/GetUserDetailByUserId/" + userId;

            ServiceResponse<UserDetailsViewModel> response = new ServiceResponse<UserDetailsViewModel>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<UserDetailsViewModel>>
                (userDetailsApiUrl, HttpMethod.Get, HttpContext.Request);

            if (response == null)
            {
                return RedirectToAction("Index");
            }

            var userDetails = response.Data;

            ViewBag.UserId = userId;
            ViewBag.Topics = GetAllTopicsByJobId(userDetails.JobId);
            ViewBag.AssignedTopics = GetAllAssignedTopics(userDetails.UserId);

            if (response.Success)
            {
                return View(response.Data);
            }
            return RedirectToAction("Index");
        }

        private IEnumerable<TopicViewModel> GetAllTopicsByJobId(int jobId)
        {
            var apiUrl = $"{endPoint}Topic/GetTopicsByJobId"
                + "?jobId=" + jobId;

            ServiceResponse<IEnumerable<TopicViewModel>> response = new ServiceResponse<IEnumerable<TopicViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<TopicViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        private IEnumerable<TrainingTopicViewModel> GetAllAssignedTopics(int userId)
        {
            var apiUrl = $"{endPoint}Trainer/GetAllTrainingTopicbyTrainerId/" + userId
                + "?page=" + 1
                + "&pageSize=" + 10;

            ServiceResponse<IEnumerable<TrainingTopicViewModel>> response = new ServiceResponse<IEnumerable<TrainingTopicViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<TrainingTopicViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        [AdminAuthorize]
        public IActionResult MonthlyAdminReport(int userId, int? month, int? year)
        {
            var apiUrl = "";

            if (month == null && year == null)
            {
                apiUrl = $"{endPoint}Admin/MonthlyAdminReport" + "?userId=" + userId;
            }
            else
            {
                apiUrl = $"{endPoint}Admin/MonthlyAdminReport" + "?userId=" + userId + "&month=" + month + "&year=" + year;

            }
            var trainers = GetAllTrainers();

            ViewBag.Trainers = trainers;
            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.UserId = userId;

            ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>> response = new ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<MonthlyAdminReportViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {

                return View(response.Data);
            }

            return View(new List<MonthlyAdminReportViewModel>());
        }

        [AdminAuthorize]
        public IActionResult DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate)
        {
            var apiUrl = "";

            if (startDate == null && endDate == null)
            {
                apiUrl = $"{endPoint}Admin/DaterangeBasedReport" + "?jobId=" + jobId;
            }
            else
            {
                apiUrl = $"{endPoint}Admin/DaterangeBasedReport" + "?jobId=" + jobId + "&startDate=" + startDate + "&endDate=" + endDate;

            }
            var jobs = GetAllJobs();

            ViewBag.Jobs = jobs;
            ViewBag.JobId = jobId;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>> response = new ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<DaterangeBasedReportViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {

                return View(response.Data);
            }

            return View(new List<DaterangeBasedReportViewModel>());
        }



    }
}
