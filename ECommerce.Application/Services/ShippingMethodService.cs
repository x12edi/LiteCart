using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ShippingMethodService : IShippingMethodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippingMethodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingMethodDto> GetByIdAsync(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingMethods.GetByIdAsync(id);
            if (shippingMethod == null)
                return null;

            return new ShippingMethodDto
            {
                Id = shippingMethod.Id,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
                EstimatedTime = shippingMethod.EstimatedTime,
                AvailabilityRegion = shippingMethod.AvailabilityRegion
            };
        }

        public async Task<IEnumerable<ShippingMethodDto>> GetAllAsync()
        {
            var shippingMethods = await _unitOfWork.ShippingMethods.GetAllAsync();
            return shippingMethods.Select(sm => new ShippingMethodDto
            {
                Id = sm.Id,
                Name = sm.Name,
                Cost = sm.Cost,
                EstimatedTime = sm.EstimatedTime,
                AvailabilityRegion = sm.AvailabilityRegion
            });
        }

        public async Task<ShippingMethodDto> CreateAsync(CreateShippingMethodDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            if (dto.Cost < 0)
                throw new ArgumentException("Cost cannot be negative.");

            var shippingMethod = new ShippingMethod
            {
                Name = dto.Name,
                Cost = dto.Cost,
                EstimatedTime = dto.EstimatedTime,
                AvailabilityRegion = dto.AvailabilityRegion
            };

            await _unitOfWork.ShippingMethods.AddAsync(shippingMethod);
            await _unitOfWork.CompleteAsync();

            return new ShippingMethodDto
            {
                Id = shippingMethod.Id,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
                EstimatedTime = shippingMethod.EstimatedTime,
                AvailabilityRegion = shippingMethod.AvailabilityRegion
            };
        }

        public async Task UpdateAsync(int id, UpdateShippingMethodDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            if (dto.Cost < 0)
                throw new ArgumentException("Cost cannot be negative.");

            var shippingMethod = await _unitOfWork.ShippingMethods.GetByIdAsync(id);
            if (shippingMethod == null)
                throw new KeyNotFoundException("Shipping method not found.");

            shippingMethod.Name = dto.Name;
            shippingMethod.Cost = dto.Cost;
            shippingMethod.EstimatedTime = dto.EstimatedTime;
            shippingMethod.AvailabilityRegion = dto.AvailabilityRegion;

            await _unitOfWork.ShippingMethods.UpdateAsync(shippingMethod);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingMethods.GetByIdAsync(id);
            if (shippingMethod == null)
                throw new KeyNotFoundException("Shipping method not found.");

            await _unitOfWork.ShippingMethods.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}