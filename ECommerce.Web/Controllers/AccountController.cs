using ECommerce.Application.Services;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        public IActionResult IsUserLoggedIn()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            return Json(userId.HasValue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}");
                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine($"Error: {e.ErrorMessage}");
                    }
                }

                //model.ErrorMessage = "Invalid input.";
                return View(model);
            }

            var users = await _userService.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);
            if (user == null)
            {
                model.ErrorMessage = "Invalid credentials.";
                return View(model);
            }

            //if (user.Status != "Active")
            //{
            //    model.ErrorMessage = "User account is not active.";
            //    return View(model);
            //}

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            var userRoles = await _userService.GetUserRolesAsync(user.Id);
            if (!userRoles.Any(ur => ur.RoleId == 1)) // Assume RoleId 1 is Admin
            {
                //model.ErrorMessage = "User is not an admin.";
                //return View(model);
                return RedirectToAction("List", "Product");
            }

            

            TempData["WelcomeMessage"] = $"Welcome back, {user.Username}!";

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}