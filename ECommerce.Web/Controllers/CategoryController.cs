using ECommerce.Application.Dtos;
using ECommerce.Application.Services;
using ECommerce.Web.Filters;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ECommerce.Web.Controllers
{
    [AdminAuthorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        
        public CategoryController(ICategoryService categoryService) { 
            this._categoryService = categoryService;
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var list = await _categoryService.GetAllAsync();
            var model = list.Select(o => new CategoryViewModel
            {
                Id = o.Id,
                Name = o.Name,
                ParentId = o.ParentId,
                ParentCategory = o.ParentCategory,
            });
            return View(model);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
            return View("_CategoryForm", new CategoryViewModel());
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories;
                return PartialView("_CategoryForm", model);
            }

            var o = new CreateCategoryDto
            {
                Name = model.Name,
                ParentId = model.ParentId
            };

            await _categoryService.CreateAsync(o);
            TempData["SuccessMessage"] = "Category created successfully.";

            return Json(new { success = true });
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var o = await _categoryService.GetByIdAsync(id);
            if(o == null)
                return NotFound();

            var model = new CategoryViewModel
            {
                Id = o.Id,
                Name = o.Name,
                ParentId = o.ParentId,
                ParentCategory = o.ParentCategory,
            };

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
            return View("_CategoryForm", model);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories;
                return PartialView("_CategoryForm", model);
            }

            var o = new UpdateCategoryDto
            {
                Name = model.Name,
                ParentId = model.ParentId
            };

            await _categoryService.UpdateAsync(model.Id, o);
            TempData["SuccessMessage"] = "Category updated successfully.";

            return Json(new { success = true });
        }

        // GET: CategoryController/Delete/5
        //public async Task<ActionResult> Delete(int id)
        //{
            
        //}

        // POST: CategoryController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var o = await _categoryService.GetByIdAsync(id);
            if (o == null)
                return NotFound();

            await _categoryService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Category deleted successfully.";

            return Json(new { success = true });
        }
    }
}
