using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<WarehouseDto> GetByIdAsync(int id)
        {
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id);
            if (warehouse == null)
                return null;

            return new WarehouseDto
            {
                Id = warehouse.Id,
                Location = warehouse.Location,
                Manager = warehouse.Manager,
                Capacity = warehouse.Capacity
            };
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllAsync()
        {
            var warehouses = await _unitOfWork.Warehouses.GetAllAsync();
            return warehouses.Select(w => new WarehouseDto
            {
                Id = w.Id,
                Location = w.Location,
                Manager = w.Manager,
                Capacity = w.Capacity
            });
        }

        public async Task<WarehouseDto> CreateAsync(CreateWarehouseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Location))
                throw new ArgumentException("Location is required.");
            if (dto.Capacity <= 0)
                throw new ArgumentException("Capacity must be positive.");

            var warehouse = new Warehouse
            {
                Location = dto.Location,
                Manager = dto.Manager,
                Capacity = dto.Capacity
            };

            await _unitOfWork.Warehouses.AddAsync(warehouse);
            await _unitOfWork.CompleteAsync();

            return new WarehouseDto
            {
                Id = warehouse.Id,
                Location = warehouse.Location,
                Manager = warehouse.Manager,
                Capacity = warehouse.Capacity
            };
        }

        public async Task UpdateAsync(int id, UpdateWarehouseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Location))
                throw new ArgumentException("Location is required.");
            if (dto.Capacity <= 0)
                throw new ArgumentException("Capacity must be positive.");

            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id);
            if (warehouse == null)
                throw new KeyNotFoundException("Warehouse not found.");

            warehouse.Location = dto.Location;
            warehouse.Manager = dto.Manager;
            warehouse.Capacity = dto.Capacity;

            await _unitOfWork.Warehouses.UpdateAsync(warehouse);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(id);
            if (warehouse == null)
                throw new KeyNotFoundException("Warehouse not found.");

            await _unitOfWork.Warehouses.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}