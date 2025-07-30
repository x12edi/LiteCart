using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationDto> GetByIdAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                return null;

            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Type = notification.Type,
                Message = notification.Message,
                Status = notification.Status,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync()
        {
            var notifications = await _unitOfWork.Notifications.GetAllAsync();
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                Message = n.Message,
                Status = n.Status,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (string.IsNullOrWhiteSpace(dto.Type) || string.IsNullOrWhiteSpace(dto.Message) ||
                string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Type, Message, and Status are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var notification = new Notification
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Message = dto.Message,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Type = notification.Type,
                Message = notification.Message,
                Status = notification.Status,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task UpdateAsync(int id, UpdateNotificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.");

            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            notification.Status = dto.Status;

            await _unitOfWork.Notifications.UpdateAsync(notification);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            await _unitOfWork.Notifications.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}