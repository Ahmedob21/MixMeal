using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MixMeal.customAuth
{
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int _roleId;
        public CustomAuthorizeAttribute(int roleId)
        {
            _roleId = roleId;
        }
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetInt32("roleSession");
            if (!userRole.HasValue || userRole.Value != _roleId)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
            return Task.CompletedTask;
        }
    }
}
