using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InventoryDto> GetByKeysAsync(int productVariantId, int variantId, int warehouseId)
        {
            var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productVariantId, variantId, warehouseId);
            if (inventory == null)
                return null;

            return new InventoryDto
            {
                ProductVariantId = inventory.VariantId,
                WarehouseId = inventory.WarehouseId,
                Quantity = inventory.Quantity
            };
        }

        public async Task<IEnumerable<InventoryDto>> GetAllAsync()
        {
            var inventories = await _unitOfWork.Inventories.GetAllAsync();
            return inventories.Select(i => new InventoryDto
            {
                ProductVariantId = i.VariantId,
                WarehouseId = i.WarehouseId,
                Quantity = i.Quantity
            });
        }

        public async Task<InventoryDto> CreateAsync(CreateInventoryDto dto)
        {
            if (dto.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            if (dto.ProductVariantId <= 0 || dto.WarehouseId <= 0)
                throw new ArgumentException("Valid ProductVariantId and WarehouseId are required.");

            var productVariant = await _unitOfWork.ProductVariants.GetByIdAsync(dto.ProductVariantId);
            if (productVariant == null)
                throw new KeyNotFoundException("Product variant not found.");

            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(dto.WarehouseId);
            if (warehouse == null)
                throw new KeyNotFoundException("Warehouse not found.");

            var inventory = new Inventory
            {
                VariantId = dto.ProductVariantId,
                WarehouseId = dto.WarehouseId,
                Quantity = dto.Quantity
            };

            await _unitOfWork.Inventories.AddAsync(inventory);
            await _unitOfWork.CompleteAsync();

            return new InventoryDto
            {
                ProductVariantId = inventory.VariantId,
                WarehouseId = inventory.WarehouseId,
                Quantity = inventory.Quantity
            };
        }

        public async Task UpdateAsync(int productId, int productVariantId, int warehouseId, UpdateInventoryDto dto)
        {
            if (dto.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productId, productVariantId, warehouseId);
            if (inventory == null)
                throw new KeyNotFoundException("Inventory not found.");

            inventory.Quantity = dto.Quantity;
            await _unitOfWork.Inventories.UpdateAsync(inventory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int productId, int productVariantId, int warehouseId)
        {
            var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productId, productVariantId, warehouseId);
            if (inventory == null)
                throw new KeyNotFoundException("Inventory not found.");

            await _unitOfWork.Inventories.DeleteByProductVariantAndWarehouseIdAsync(productId, productVariantId, warehouseId);
            await _unitOfWork.CompleteAsync();
        }
    }
}