using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductVariantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductVariantDto> GetByIdAsync(int id)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(id);
            if (variant == null)
                return null;

            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                Size = variant.Size,
                Color = variant.Color,
                Price = variant.Price,
                Stock = variant.Stock
            };
        }

        public async Task<IEnumerable<ProductVariantDto>> GetAllAsync()
        {
            var variants = await _unitOfWork.ProductVariants.GetAllAsync();
            return variants.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                Size = v.Size,
                Color = v.Color,
                Price = v.Price,
                Stock = v.Stock
            });
        }

        public async Task<ProductVariantDto> CreateAsync(CreateProductVariantDto dto)
        {
            if (dto.ProductId <= 0)
                throw new ArgumentException("Valid ProductId is required.");
            if (dto.Price < 0)
                throw new ArgumentException("Price cannot be negative.");
            if (dto.Stock < 0)
                throw new ArgumentException("Stock cannot be negative.");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            var variant = new ProductVariant
            {
                ProductId = dto.ProductId,
                Size = dto.Size,
                Color = dto.Color,
                Price = dto.Price,
                Stock = dto.Stock
            };

            await _unitOfWork.ProductVariants.AddAsync(variant);
            await _unitOfWork.CompleteAsync();

            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                Size = variant.Size,
                Color = variant.Color,
                Price = variant.Price,
                Stock = variant.Stock
            };
        }

        public async Task UpdateAsync(int id, UpdateProductVariantDto dto)
        {
            if (dto.ProductId <= 0)
                throw new ArgumentException("Valid ProductId is required.");
            if (dto.Price < 0)
                throw new ArgumentException("Price cannot be negative.");
            if (dto.Stock < 0)
                throw new ArgumentException("Stock cannot be negative.");

            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(id);
            if (variant == null)
                throw new KeyNotFoundException("Product variant not found.");

            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            variant.ProductId = dto.ProductId;
            variant.Size = dto.Size;
            variant.Color = dto.Color;
            variant.Price = dto.Price;
            variant.Stock = dto.Stock;

            await _unitOfWork.ProductVariants.UpdateAsync(variant);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(id);
            if (variant == null)
                throw new KeyNotFoundException("Product variant not found.");

            await _unitOfWork.ProductVariants.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}