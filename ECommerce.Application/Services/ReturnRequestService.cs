using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReturnRequestDto> GetByIdAsync(int id)
        {
            var returnRequest = await _unitOfWork.ReturnRequests.GetByIdAsync(id);
            if (returnRequest == null)
                return null;

            return new ReturnRequestDto
            {
                Id = returnRequest.Id,
                OrderItemId = returnRequest.OrderItemId,
                Reason = returnRequest.Reason,
                Status = returnRequest.Status,
                ResolutionType = returnRequest.ResolutionType
            };
        }

        public async Task<IEnumerable<ReturnRequestDto>> GetAllAsync()
        {
            var returnRequests = await _unitOfWork.ReturnRequests.GetAllAsync();
            return returnRequests.Select(rr => new ReturnRequestDto
            {
                Id = rr.Id,
                OrderItemId = rr.OrderItemId,
                Reason = rr.Reason,
                Status = rr.Status,
                ResolutionType = rr.ResolutionType
            });
        }

        public async Task<ReturnRequestDto> CreateAsync(CreateReturnRequestDto dto)
        {
            if (dto.OrderItemId <= 0)
                throw new ArgumentException("Valid OrderItemId is required.");
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Reason is required.");
            if (string.IsNullOrWhiteSpace(dto.ResolutionType))
                throw new ArgumentException("ResolutionType is required.");

            var orderItem = await _unitOfWork.OrderItems.GetByIdAsync(dto.OrderItemId);
            if (orderItem == null)
                throw new KeyNotFoundException("Order item not found.");

            var returnRequest = new ReturnRequest
            {
                OrderItemId = dto.OrderItemId,
                Reason = dto.Reason,
                Status = "Pending",
                ResolutionType = dto.ResolutionType
            };

            await _unitOfWork.ReturnRequests.AddAsync(returnRequest);
            await _unitOfWork.CompleteAsync();

            return new ReturnRequestDto
            {
                Id = returnRequest.Id,
                OrderItemId = returnRequest.OrderItemId,
                Reason = returnRequest.Reason,
                Status = returnRequest.Status,
                ResolutionType = returnRequest.ResolutionType
            };
        }

        public async Task UpdateAsync(int id, UpdateReturnRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");
            if (string.IsNullOrWhiteSpace(dto.ResolutionType))
                throw new ArgumentException("ResolutionType is required.");

            var returnRequest = await _unitOfWork.ReturnRequests.GetByIdAsync(id);
            if (returnRequest == null)
                throw new KeyNotFoundException("Return request not found.");

            returnRequest.Status = dto.Status;
            returnRequest.ResolutionType = dto.ResolutionType;

            await _unitOfWork.ReturnRequests.UpdateAsync(returnRequest);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var returnRequest = await _unitOfWork.ReturnRequests.GetByIdAsync(id);
            if (returnRequest == null)
                throw new KeyNotFoundException("Return request not found.");

            await _unitOfWork.ReturnRequests.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}