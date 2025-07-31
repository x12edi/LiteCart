using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return null;

            var productCategories = await _unitOfWork.ProductCategories.GetByProductIdAsync(id);
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Status = product.Status,
                Images = product.Images,
                CategoryIds = productCategories.Select(pc => pc.CategoryId).ToList()
            };
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var result = new List<ProductDto>();
            foreach (var product in products)
            {
                var productCategories = await _unitOfWork.ProductCategories.GetByProductIdAsync(product.Id);
                result.Add(new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    SKU = product.SKU,
                    Status = product.Status,
                    Images = product.Images,
                    CategoryIds = productCategories.Select(pc => pc.CategoryId).ToList()
                });
            }
            return result;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required.");
            if (dto.Price < 0)
                throw new ArgumentException("Base price cannot be negative.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                SKU = dto.SKU,
                Status = dto.Status,
                Images = dto.Images
            };

            await _unitOfWork.Products.AddAsync(product);
            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                foreach (var categoryId in dto.CategoryIds)
                {
                    await _unitOfWork.ProductCategories.AddAsync(new ProductCategories
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Status = product.Status,
                Images = product.Images,
                CategoryIds = (dto.CategoryIds ?? Enumerable.Empty<int>()).ToList()
            };
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required.");
            if (dto.Price < 0)
                throw new ArgumentException("Base price cannot be negative.");

            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.SKU = dto.SKU;
            product.Status = dto.Status;
            product.Images = dto.Images;

            // Update categories
            await _unitOfWork.ProductCategories.DeleteByProductIdAsync(id);
            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                foreach (var categoryId in dto.CategoryIds)
                {
                    await _unitOfWork.ProductCategories.AddAsync(new ProductCategories
                    {
                        ProductId = id,
                        CategoryId = categoryId
                    });
                }
            }

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            await _unitOfWork.ProductCategories.DeleteByProductIdAsync(id);
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}