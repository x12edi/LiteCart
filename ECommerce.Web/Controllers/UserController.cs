using ECommerce.Application.Dtos;
using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        { 
            _userService = userService;
        }
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var list = await _userService.GetAllAsync();
            var model = list.Select(o => new UserViewModel
            {
                Id = o.Id,
                Username = o.Username,
                Email = o.Email,
                Phone = o.Phone,
                CreatedAt = o.CreatedAt,
                IsActive = o.IsActive,
                IsCustomer = o.IsCustomer,
            });
            return View(model);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View("_UserForm", new UserViewModel());
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel model)
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
                return PartialView("_UserForm", model);

            }

            var o = new CreateUserDto
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                Phone = model.Phone,
                IsActive = model.IsActive,
                IsCustomer = model.IsCustomer,
            };

            await _userService.CreateAsync(o);
            TempData["SuccessMessage"] = "User created successfully.";

            return Json(new { success = true });
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var o = await _userService.GetByIdAsync(id);
            if (o == null)
                return NotFound();

            var model = new UserViewModel
            {
                Id = o.Id,
                Username = o.Username,
                Email = o.Email,
                Phone = o.Phone,
                CreatedAt = o.CreatedAt,
                IsActive = o.IsActive,
                IsCustomer = o.IsCustomer,
            };
            return View("_UserForm", model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserViewModel model)
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
                return PartialView("_UserForm", model);
            }

            var o = new UpdateUserDto
            {
                Username = model.Username,
                Email = model.Email,
                Phone = model.Phone,
                IsActive = model.IsActive,
                IsCustomer = model.IsCustomer,
            };

            await _userService.UpdateAsync(model.Id, o);
            TempData["SuccessMessage"] = "User updated successfully.";

            return Json(new { success = true });
        }

        // GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: UserController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var o = await _userService.GetByIdAsync(id);
            if (o == null)
                return NotFound();

            await _userService.DeleteAsync(id);
            TempData["SuccessMessage"] = "User deleted successfully.";

            return Json(new { success = true });
        }
    }
}
