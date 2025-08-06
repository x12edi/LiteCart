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

            var categories = await _unitOfWork.ProductCategories.GetAllAsync();
            var variants = await _unitOfWork.ProductVariants.GetByProductIdAsync(id);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Status = product.Status,
                Images = product.Images,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryIds = categories.Where(pc => pc.ProductId == id).Select(pc => pc.CategoryId).ToList(),
                Variants = variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Size = v.Size,
                    Color = v.Color,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            };
        }


        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var categories = await _unitOfWork.ProductCategories.GetAllAsync();
            var variants = await _unitOfWork.ProductVariants.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SKU = p.SKU,
                Status = p.Status,
                Images = p.Images,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CategoryIds = categories.Where(pc => pc.ProductId == p.Id).Select(pc => pc.CategoryId).ToList(),
                Variants = variants.Where(v => v.ProductId == p.Id).Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Size = v.Size,
                    Color = v.Color,
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            }).ToList();
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

            if (dto.Variants != null && dto.Variants.Any())
            {
                foreach (var variant in dto.Variants)
                {
                    if (variant.Price < 0)
                        throw new ArgumentException($"Variant price for {variant.Size} - {variant.Color} cannot be negative.");
                    if (variant.Stock < 0)
                        throw new ArgumentException($"Variant stock for {variant.Size} - {variant.Color} cannot be negative.");

                    await _unitOfWork.ProductVariants.AddAsync(new ProductVariant
                    {
                        ProductId = product.Id,
                        Size = variant.Size,
                        Color = variant.Color,
                        Price = variant.Price,
                        Stock = variant.Stock
                    });
                }
            }

            //await _unitOfWork.CompleteAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Status = product.Status,
                Images = product.Images,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryIds = (dto.CategoryIds ?? Enumerable.Empty<int>()).ToList(),
                Variants = (dto.Variants ?? Enumerable.Empty<ProductVariantDto>()).ToList()
            };

        }

        public async Task UpdateAsync(int productId, UpdateProductDto dto)
        {
            //using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = new Product
                {
                    Id = productId,
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    SKU = dto.SKU,
                    Status = dto.Status,
                    Images = dto.Images,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Products.UpdateAsync(product);

                await _unitOfWork.ProductCategories.DeleteByProductIdAsync(productId);
                if (dto.CategoryIds != null && dto.CategoryIds.Any())
                {
                    foreach (var categoryId in dto.CategoryIds)
                    {
                        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                        if (category == null)
                            throw new ArgumentException($"Category ID {categoryId} does not exist.");

                        await _unitOfWork.ProductCategories.AddAsync(new ProductCategories
                        {
                            ProductId = productId,
                            CategoryId = categoryId
                        });
                    }
                }

                await _unitOfWork.ProductVariants.DeleteByProductIdAsync(productId);
                if (dto.Variants != null && dto.Variants.Any())
                {
                    foreach (var variant in dto.Variants)
                    {
                        if (variant.Price < 0)
                            throw new ArgumentException($"Variant price for {variant.Size} - {variant.Color} cannot be negative.");
                        if (variant.Stock < 0)
                            throw new ArgumentException($"Variant stock for {variant.Size} - {variant.Color} cannot be negative.");

                        await _unitOfWork.ProductVariants.AddAsync(new ProductVariant
                        {
                            ProductId = productId,
                            Size = variant.Size,
                            Color = variant.Color,
                            Price = variant.Price,
                            Stock = variant.Stock
                        });
                    }
                }

                await _unitOfWork.CompleteAsync();
                //await transaction.CommitAsync();
            }
            catch (Exception)
            {
                //await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            await _unitOfWork.ProductCategories.DeleteByProductIdAsync(id);
            await _unitOfWork.ProductVariants.DeleteByProductIdAsync(id);
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}