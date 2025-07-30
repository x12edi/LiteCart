using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (dto.OrderItems == null || !dto.OrderItems.Any())
                throw new ArgumentException("At least one order item is required.");

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in dto.OrderItems)
            {
                if (itemDto.Quantity <= 0)
                    throw new ArgumentException("Quantity must be positive.");

                var variant = await _unitOfWork.ProductVariants.GetByIdAsync(itemDto.ProductVariantId);
                if (variant == null)
                    throw new KeyNotFoundException($"Product variant {itemDto.ProductVariantId} not found.");
                if (variant.Stock < itemDto.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product variant {itemDto.ProductVariantId}.");

                var orderItem = new OrderItem
                {
                    ProductVariantId = itemDto.ProductVariantId,
                    Quantity = itemDto.Quantity,
                    PriceAtTime = variant.Price
                };
                totalAmount += variant.Price * itemDto.Quantity;
                orderItems.Add(orderItem);
            }

            var order = new Order
            {
                UserId = dto.UserId,
                TotalAmount = totalAmount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Orders.AddAsync(order);
            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                await _unitOfWork.OrderItems.AddAsync(orderItem);
                // Update stock
                var variant = await _unitOfWork.ProductVariants.GetByIdAsync(orderItem.ProductVariantId);
                variant.Stock -= orderItem.Quantity;
                await _unitOfWork.ProductVariants.UpdateAsync(variant);
            }

            await _unitOfWork.CompleteAsync();

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task UpdateAsync(int id, UpdateOrderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Status = dto.Status;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            // Delete related order items
            var orderItems = await _unitOfWork.OrderItems.GetAllAsync();
            foreach (var item in orderItems.Where(oi => oi.OrderId == id))
            {
                await _unitOfWork.OrderItems.DeleteAsync(item.Id);
            }

            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}