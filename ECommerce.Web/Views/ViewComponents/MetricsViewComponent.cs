using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Web.ViewComponents
{
    public class MetricsViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerSupportTicketService _supportTicketService;

        public MetricsViewComponent(
            IUserService userService,
            IOrderService orderService,
            IProductService productService,
            ICustomerSupportTicketService supportTicketService)
        {
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
            _supportTicketService = supportTicketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userCount = (await _userService.GetAllAsync()).Count();
            var orderCount = (await _orderService.GetAllAsync()).Count();
            var productCount = (await _productService.GetAllAsync()).Count();
            var ticketCount = (await _supportTicketService.GetAllAsync()).Count();

            var model = new
            {
                UserCount = userCount,
                OrderCount = orderCount,
                ProductCount = productCount,
                TicketCount = ticketCount
            };

            return View(model);
        }
    }
}