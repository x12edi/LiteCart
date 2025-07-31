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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

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
    }
}