using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TrainingRecordSystemMVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TrainerAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpContext context1 = context.HttpContext;
            var userId = Convert.ToInt32(context1.Request.Cookies["UserId"]);

            if (userId > 2)
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
