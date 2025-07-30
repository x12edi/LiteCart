using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                return null;

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                TransactionId = payment.TransactionId
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                Method = p.Method,
                Status = p.Status,
                TransactionId = p.TransactionId
            });
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            if (dto.OrderId <= 0)
                throw new ArgumentException("Valid OrderId is required.");
            if (dto.Amount <= 0)
                throw new ArgumentException("Amount must be positive.");
            if (string.IsNullOrWhiteSpace(dto.Method))
                throw new ArgumentException("Payment method is required.");
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var order = await _unitOfWork.Orders.GetByIdAsync(dto.OrderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                Method = dto.Method,
                Status = dto.Status,
                TransactionId = dto.TransactionId
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CompleteAsync();

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                TransactionId = payment.TransactionId
            };
        }

        public async Task UpdateAsync(int id, UpdatePaymentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            payment.Status = dto.Status;
            payment.TransactionId = dto.TransactionId;

            await _unitOfWork.Payments.UpdateAsync(payment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            await _unitOfWork.Payments.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}