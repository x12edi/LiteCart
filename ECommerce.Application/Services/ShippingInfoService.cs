using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ShippingInfoService : IShippingInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippingInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShippingInfoDto> GetByIdAsync(int id)
        {
            var shippingInfo = await _unitOfWork.ShippingInfos.GetByIdAsync(id);
            if (shippingInfo == null)
                return null;

            return new ShippingInfoDto
            {
                Id = shippingInfo.Id,
                OrderId = shippingInfo.OrderId,
                AddressId = shippingInfo.AddressId,
                ShippingMethodId = shippingInfo.ShippingMethodId,
                Status = shippingInfo.Status,
                TrackingNumber = shippingInfo.TrackingNumber
            };
        }

        public async Task<IEnumerable<ShippingInfoDto>> GetAllAsync()
        {
            var shippingInfos = await _unitOfWork.ShippingInfos.GetAllAsync();
            return shippingInfos.Select(si => new ShippingInfoDto
            {
                Id = si.Id,
                OrderId = si.OrderId,
                AddressId = si.AddressId,
                ShippingMethodId = si.ShippingMethodId,
                Status = si.Status,
                TrackingNumber = si.TrackingNumber
            });
        }

        public async Task<ShippingInfoDto> CreateAsync(CreateShippingInfoDto dto)
        {
            if (dto.OrderId <= 0 || dto.AddressId <= 0 || dto.ShippingMethodId <= 0)
                throw new ArgumentException("Valid OrderId, AddressId, and ShippingMethodId are required.");
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var order = await _unitOfWork.Orders.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            var address = await _unitOfWork.Addresses.GetByIdAsync(dto.AddressId);
            if (address == null)
                throw new KeyNotFoundException("Address not found.");

            var shippingMethod = await _unitOfWork.ShippingMethods.GetByIdAsync(dto.ShippingMethodId);
            if (shippingMethod == null)
                throw new KeyNotFoundException("Shipping method not found.");

            var shippingInfo = new ShippingInfo
            {
                OrderId = dto.OrderId,
                AddressId = dto.AddressId,
                ShippingMethodId = dto.ShippingMethodId,
                Status = dto.Status,
                TrackingNumber = dto.TrackingNumber
            };

            await _unitOfWork.ShippingInfos.AddAsync(shippingInfo);
            await _unitOfWork.CompleteAsync();

            return new ShippingInfoDto
            {
                Id = shippingInfo.Id,
                OrderId = shippingInfo.OrderId,
                AddressId = shippingInfo.AddressId,
                ShippingMethodId = shippingInfo.ShippingMethodId,
                Status = shippingInfo.Status,
                TrackingNumber = shippingInfo.TrackingNumber
            };
        }

        public async Task UpdateAsync(int id, UpdateShippingInfoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var shippingInfo = await _unitOfWork.ShippingInfos.GetByIdAsync(id);
            if (shippingInfo == null)
                throw new KeyNotFoundException("Shipping info not found.");

            shippingInfo.Status = dto.Status;
            shippingInfo.TrackingNumber = dto.TrackingNumber;

            await _unitOfWork.ShippingInfos.UpdateAsync(shippingInfo);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shippingInfo = await _unitOfWork.ShippingInfos.GetByIdAsync(id);
            if (shippingInfo == null)
                throw new KeyNotFoundException("Shipping info not found.");

            await _unitOfWork.ShippingInfos.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}