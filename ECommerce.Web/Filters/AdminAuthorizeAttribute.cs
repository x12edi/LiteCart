using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ECommerce.Web.Filters
{
    public class AdminAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService _userService;

        public AdminAuthorizeFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var userRoles = await _userService.GetUserRolesAsync(userId.Value);
            if (!userRoles.Any(ur => ur.RoleId == 1)) // Assume RoleId 1 is Admin
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public class AdminAuthorizeAttribute : TypeFilterAttribute
    {
        public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizeFilter))
        {
        }
    }

}