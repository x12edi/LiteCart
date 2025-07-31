using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;

        public DashboardController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetByIdAsync(userId.Value);
            var model = new DashboardViewModel
            {
                Username = user.Username,
                WelcomeMessage = TempData["WelcomeMessage"]?.ToString() ?? $"Welcome, {user.Username}!"
            };

            return View(model);
        }
    }
}