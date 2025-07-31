using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.Web.ViewComponents
{
    public class MetricsViewModel
    {
        public int UserCount { get; set; }
        public int OrderCount { get; set; }
        public int ProductCount { get; set; }
        public int TicketCount { get; set; }
    }

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
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _supportTicketService = supportTicketService ?? throw new ArgumentNullException(nameof(supportTicketService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var userCount = (await _userService.GetAllAsync()).Count();
                var orderCount = (await _orderService.GetAllAsync()).Count();
                var productCount = (await _productService.GetAllAsync()).Count();
                var ticketCount = (await _supportTicketService.GetAllAsync()).Count();

                var model = new MetricsViewModel
                {
                    UserCount = userCount,
                    OrderCount = orderCount,
                    ProductCount = productCount,
                    TicketCount = ticketCount
                };

                return View("Default", model);
            }
            catch (Exception ex)
            {
                // Log error (in production, use a logger like Serilog)
                Console.WriteLine($"MetricsViewComponent error: {ex.Message}");
                return Content("Error loading metrics. Please try again.");
            }
        }
    }
}