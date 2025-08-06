using ECommerce.Application.Dtos;
using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly IProductVariantService _productVariantService;

        public ProductController(IProductService productService, ICategoryService categoryService, ICartService cartService, IProductVariantService productVariantService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
            _productVariantService = productVariantService;
        }

        public async Task<IActionResult> List(string searchQuery = "", string categoryIds = "", string sortOption = "Newest", int pageNumber = 1)
        {
            var model = new ProductListViewModel
            {
                SearchQuery = searchQuery,
                SelectedCategoryIds = string.IsNullOrEmpty(categoryIds) ? new List<int>() : categoryIds.Split(',').Select(int.Parse).ToList(),
                SortOption = sortOption,
                PageNumber = pageNumber
            };

            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                              p.SKU.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (model.SelectedCategoryIds.Any())
            {
                products = products.Where(p => p.CategoryIds.Any(cid => model.SelectedCategoryIds.Contains(cid))).ToList();
            }

            // Apply sorting
            switch (sortOption)
            {
                case "PriceAsc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "PriceDesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "NameAsc":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "NameDesc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Newest":
                default:
                    products = products.OrderByDescending(p => p.CreatedAt).ToList();
                    break;
            }

            // Pagination
            model.TotalProducts = products.Count();
            products = products.Skip((pageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();

            model.Products = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SKU = p.SKU,
                Status = p.Status,
                ImageBase64 = p.Images != null ? Convert.ToBase64String(p.Images) : null,
                CategoryIds = p.CategoryIds,
                ProductVariants = p.Variants.Select(v => new ProductVariantViewModel
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Size = v.Size,
                    Color = v.Color,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            }).ToList();


            model.Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productVariantId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Please log in to add items to cart." });
            }

            var variant = await _productVariantService.GetByIdAsync(productVariantId);
            if (variant == null || variant.Stock <= 0)
            {
                return Json(new { success = false, message = "Variant not available." });
            }

            var product = await _productService.GetByIdAsync(variant.ProductId);
            if (product == null || product.Status != "Active")
            {
                return Json(new { success = false, message = "Product not available." });
            }

            var cart = await _cartService.GetByUserIdOrSessionAsync(userId, string.Empty);
            if (cart == null)
            { 
                cart.UserId = userId;
                cart.SessionId = HttpContext.Session.Id;
                //cart.Items = new List<CartItemDto>();
            }

            var cartItem = new CartItemDto
            {
                CartId = cart.Id,
                ProductVariantId = productVariantId, // Assuming no variants for simplicity
                Quantity = quantity,
                PriceAtTime = variant.Price
            };

            //cart.Items.Append(cartItem);

            //await _cartService.AddItemAsync(userId, cartItem);
            return Json(new { success = true, message = "Product added to cart." });
        }


        #region admin section
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            var model = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SKU = p.SKU,
                Status = p.Status,
                ImageBase64 = p.Images != null ? Convert.ToBase64String(p.Images) : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CategoryIds = p.CategoryIds
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
            return PartialView("_ProductForm", new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
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

                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories;
                return PartialView("_ProductForm", model);
            }

            byte[] imageBytes = null;
            if (model.ImageFile != null)
            {
                using var stream = new MemoryStream();
                await model.ImageFile.CopyToAsync(stream);
                imageBytes = stream.ToArray();
            }

            var productDto = new CreateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                SKU = model.SKU,
                Status = model.Status,
                Images = imageBytes,
                CategoryIds = model.CategoryIds
            };

            await _productService.CreateAsync(productDto);
            TempData["SuccessMessage"] = "Product created successfully.";
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Status = product.Status,
                ImageBase64 = product.Images != null ? Convert.ToBase64String(product.Images) : null,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryIds = product.CategoryIds
            };

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
            return PartialView("_ProductForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories;
                return PartialView("_ProductForm", model);
            }

            byte[] imageBytes = null;
            if (model.ImageFile != null)
            {
                using var stream = new MemoryStream();
                await model.ImageFile.CopyToAsync(stream);
                imageBytes = stream.ToArray();
            }
            else if (!string.IsNullOrEmpty(model.ImageBase64))
            {
                imageBytes = Convert.FromBase64String(model.ImageBase64);
            }
            else
            {
                var p = await _productService.GetByIdAsync(model.Id);
                imageBytes = p.Images;
            }

            var productDto = new UpdateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                SKU = model.SKU,
                Status = model.Status,
                Images = imageBytes,
                CategoryIds = model.CategoryIds
            };

            await _productService.UpdateAsync(model.Id, productDto);
            TempData["SuccessMessage"] = "Product updated successfully.";
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            await _productService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Product deleted successfully.";
            return Json(new { success = true });
        }

        #endregion

    }
}