using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;

        public CartController(ICartService cartService, IProductService productService, IProductVariantService productVariantService)
        {
            _cartService = cartService;
            _productService = productService;
            _productVariantService = productVariantService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var cart = await _cartService.GetByUserIdOrSessionAsync(userId,string.Empty);
            var model = new CartViewModel();

            if (cart != null)
            {
                HttpContext.Session.SetInt32("CartId", cart.Id);
                model.CartId = cart.Id;
                foreach (var item in cart.Items)
                {
                    var variant = await _productVariantService.GetByIdAsync(item.ProductVariantId);
                    if (variant == null)
                        continue;

                    var product = await _productService.GetByIdAsync(variant.ProductId);
                    if (product == null)
                        continue;

                    model.Items.Add(new CartItemViewModel
                    {
                        ProductVariantId = item.ProductVariantId,
                        ProductId = variant.ProductId,
                        ProductName = product.Name,
                        Size = variant.Size,
                        Color = variant.Color,
                        PriceAtTime = item.PriceAtTime,
                        Quantity = item.Quantity,
                        ImageBase64 = product.Images != null ? Convert.ToBase64String(product.Images) : null
                    });
                }
                model.TotalPrice = model.Items.Sum(i => i.Subtotal);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(int productVariantId, int quantity)
        {
            try
            {
                int cartId = (int) HttpContext.Session.GetInt32("CartId");
                await _cartService.UpdateItemAsync(cartId, productVariantId, quantity);
                return Json(new { success = true, message = "Cart updated successfully." });
            }
            catch (System.InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = "Error updating cart." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int productVariantId)
        {
            try
            {
                int cartId = (int)HttpContext.Session.GetInt32("CartId");
                await _cartService.RemoveItemAsync(cartId, productVariantId);
                return Json(new { success = true, message = "Item removed from cart." });
            }
            catch (System.InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = "Error removing item." });
            }
        }
    }
}