using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrainingRecordSystemMVC.Implementation;
using TrainingRecordSystemMVC.Infrastructure;

namespace TrainingRecordSystemMVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext context1 = context.HttpContext;
            var userId = Convert.ToInt32(context1.Request.Cookies["UserId"]);

            if (userId == 1)
            {
                   return;   
            }
            else
            {
                context1.Response.Cookies.Delete("jwtToken");
                context1.Response.Cookies.Delete("userId");
            }

            // Unauthorized access, redirect or return unauthorized result
            context.Result = new UnauthorizedResult();
        }
    }
}
