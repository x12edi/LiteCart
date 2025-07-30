using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                //Description = category.Description
            };
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                //Description = c.Description
            });
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required.");

            var category = new Category
            {
                Name = dto.Name,
                //Description = dto.Description
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                //Description = category.Description
            };
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Category name is required.");

            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            category.Name = dto.Name;
            //category.Description = dto.Description;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            await _unitOfWork.ProductCategories.DeleteByCategoryIdAsync(id);
            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}