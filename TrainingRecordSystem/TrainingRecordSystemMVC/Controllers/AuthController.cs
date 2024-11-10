using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TrainingRecordSystemMVC.Infrastructure;
using TrainingRecordSystemMVC.ViewModels;

namespace TrainingRecordSystemMVC.Controllers
{
    public class AuthController : Controller
    {

        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;

        public AuthController(IHttpClientService httpClientService, IConfiguration configuration, IJwtTokenHandler tokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
                    string token = serviceResponse.Data;

                  

                    //Below code if we want to access cookie in a ajax call.
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });

                    var jwtToken = _tokenHandler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.First(claim => claim.Type == "UserId").Value;

                    Response.Cookies.Append("userId", userId, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(1),
                    });

                    //TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                    }
                    return RedirectToAction("Login");
                }


            }
            return View(viewModel);
        }


        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwtToken");
            Response.Cookies.Delete("userId");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.LoginId = @User.Identity.Name;
            return View(model);
        }

        [HttpPost]
        //[ExcludeFromCodeCoverage]
        public IActionResult ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ChangePassword";
                var response = _httpClientService.PutHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    if (serviceResponse?.Message == string.Empty)
                    {
                        TempData["successMessage"] = "Password updated successfully";
                    }
                    else
                    {
                        TempData["successMessage"] = serviceResponse?.Message;
                    }

                    return RedirectToAction("Logout","Auth");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["errorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                    }
                }

                return RedirectToAction("ChangePassword");

            }
            return View(viewModel);
        }

    }
}
