using ECommerce.Application.Dtos;
using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            var users = await _userService.GetAllAsync();
            var model = orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                UserEmail = users.FirstOrDefault(u => u.Id == o.UserId)?.Email ?? "Unknown",
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(order.UserId);
            var model = new OrderViewModel
            {
                Id = order.Id,
                UserId = order.UserId,
                UserEmail = user?.Email ?? "Unknown",
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };

            return PartialView("_OrderForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_OrderForm", model);
            }

            var orderDto = new UpdateOrderDto
            {
                Status = model.Status,
            };

            await _orderService.UpdateAsync(model.Id, orderDto);
            TempData["SuccessMessage"] = "Order updated successfully.";
            return Json(new { success = true });
        }
    }
}