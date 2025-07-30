using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockMovementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StockMovementDto> GetByIdAsync(int id)
        {
            var stockMovement = await _unitOfWork.StockMovements.GetByIdAsync(id);
            if (stockMovement == null)
                return null;

            return new StockMovementDto
            {
                Id = stockMovement.Id,
                ProductVariantId = stockMovement.ProductVariantId,
                FromWarehouseId = stockMovement.FromWarehouseId,
                ToWarehouseId = stockMovement.ToWarehouseId,
                Quantity = stockMovement.Quantity,
                Reason = stockMovement.Reason,
                Timestamp = stockMovement.Timestamp
            };
        }

        public async Task<IEnumerable<StockMovementDto>> GetAllAsync()
        {
            var stockMovements = await _unitOfWork.StockMovements.GetAllAsync();
            return stockMovements.Select(sm => new StockMovementDto
            {
                Id = sm.Id,
                ProductVariantId = sm.ProductVariantId,
                FromWarehouseId = sm.FromWarehouseId,
                ToWarehouseId = sm.ToWarehouseId,
                Quantity = sm.Quantity,
                Reason = sm.Reason,
                Timestamp = sm.Timestamp
            });
        }

        public async Task<StockMovementDto> CreateAsync(CreateStockMovementDto dto)
        {
            if (dto.ProductVariantId <= 0)
                throw new ArgumentException("Valid ProductVariantId is required.");
            if (dto.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Reason is required.");

            var productVariant = await _unitOfWork.ProductVariants.GetByIdAsync(dto.ProductVariantId);
            if (productVariant == null)
                throw new KeyNotFoundException("Product variant not found.");

            if (dto.FromWarehouseId.HasValue)
            {
                var fromWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(dto.FromWarehouseId.Value);
                if (fromWarehouse == null)
                    throw new KeyNotFoundException("From warehouse not found.");

                var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productVariant.ProductId, dto.ProductVariantId, dto.FromWarehouseId.Value);
                if (inventory == null || inventory.Quantity < dto.Quantity)
                    throw new InvalidOperationException("Insufficient stock in source warehouse.");
            }

            if (dto.ToWarehouseId.HasValue)
            {
                var toWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(dto.ToWarehouseId.Value);
                if (toWarehouse == null)
                    throw new KeyNotFoundException("To warehouse not found.");
            }

            var stockMovement = new StockMovement
            {
                ProductVariantId = dto.ProductVariantId,
                FromWarehouseId = dto.FromWarehouseId,
                ToWarehouseId = dto.ToWarehouseId,
                Quantity = dto.Quantity,
                Reason = dto.Reason,
                Timestamp = DateTime.UtcNow
            };

            // Update inventory
            if (dto.FromWarehouseId.HasValue)
            {
                var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productVariant.ProductId,dto.ProductVariantId, dto.FromWarehouseId.Value);
                inventory.Quantity -= dto.Quantity;
                await _unitOfWork.Inventories.UpdateAsync(inventory);
            }

            if (dto.ToWarehouseId.HasValue)
            {
                var inventory = await _unitOfWork.Inventories.GetByProductVariantAndWarehouseIdAsync(productVariant.ProductId, dto.ProductVariantId, dto.ToWarehouseId.Value);
                if (inventory == null)
                {
                    inventory = new Inventory
                    {
                        VariantId = dto.ProductVariantId,
                        WarehouseId = dto.ToWarehouseId.Value,
                        Quantity = dto.Quantity
                    };
                    await _unitOfWork.Inventories.AddAsync(inventory);
                }
                else
                {
                    inventory.Quantity += dto.Quantity;
                    await _unitOfWork.Inventories.UpdateAsync(inventory);
                }
            }

            await _unitOfWork.StockMovements.AddAsync(stockMovement);
            await _unitOfWork.CompleteAsync();

            return new StockMovementDto
            {
                Id = stockMovement.Id,
                ProductVariantId = stockMovement.ProductVariantId,
                FromWarehouseId = stockMovement.FromWarehouseId,
                ToWarehouseId = stockMovement.ToWarehouseId,
                Quantity = stockMovement.Quantity,
                Reason = stockMovement.Reason,
                Timestamp = stockMovement.Timestamp
            };
        }

        public async Task DeleteAsync(int id)
        {
            var stockMovement = await _unitOfWork.StockMovements.GetByIdAsync(id);
            if (stockMovement == null)
                throw new KeyNotFoundException("Stock movement not found.");

            await _unitOfWork.StockMovements.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}