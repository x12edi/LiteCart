using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategoriesDto> GetByKeysAsync(int productId, int categoryId)
        {
            var productCategory = await _unitOfWork.ProductCategories.GetByProductAndCategoryIdAsync(productId, categoryId);
            if (productCategory == null)
                return null;

            return new ProductCategoriesDto
            {
                ProductId = productCategory.ProductId,
                CategoryId = productCategory.CategoryId
            };
        }

        public async Task<IEnumerable<ProductCategoriesDto>> GetAllAsync()
        {
            var productCategories = await _unitOfWork.ProductCategories.GetAllAsync();
            return productCategories.Select(pc => new ProductCategoriesDto
            {
                ProductId = pc.ProductId,
                CategoryId = pc.CategoryId
            });
        }

        public async Task<ProductCategoriesDto> CreateAsync(CreateProductCategoriesDto dto)
        {
            if (dto.ProductId <= 0 || dto.CategoryId <= 0)
                throw new ArgumentException("Valid ProductId and CategoryId are required.");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var productCategory = new ProductCategories
            {
                ProductId = dto.ProductId,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.ProductCategories.AddAsync(productCategory);
            await _unitOfWork.CompleteAsync();

            return new ProductCategoriesDto
            {
                ProductId = productCategory.ProductId,
                CategoryId = productCategory.CategoryId
            };
        }

        public async Task DeleteAsync(int productId, int categoryId)
        {
            var productCategory = await _unitOfWork.ProductCategories.GetByProductAndCategoryIdAsync(productId, categoryId);
            if (productCategory == null)
                throw new KeyNotFoundException("Product category mapping not found.");

            await _unitOfWork.ProductCategories.DeleteByProductAndCategoryIdAsync(productId, categoryId);
            await _unitOfWork.CompleteAsync();
        }
    }
}