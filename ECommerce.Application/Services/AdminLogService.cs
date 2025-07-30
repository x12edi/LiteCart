using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class AdminLogService : IAdminLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminLogDto> GetByIdAsync(int id)
        {
            var adminLog = await _unitOfWork.AdminLogs.GetByIdAsync(id);
            if (adminLog == null)
                return null;

            return new AdminLogDto
            {
                Id = adminLog.Id,
                UserId = adminLog.UserId,
                Action = adminLog.Action,
                Entity = adminLog.Entity,
                EntityId = adminLog.EntityId,
                Timestamp = adminLog.Timestamp
            };
        }

        public async Task<IEnumerable<AdminLogDto>> GetAllAsync()
        {
            var adminLogs = await _unitOfWork.AdminLogs.GetAllAsync();
            return adminLogs.Select(al => new AdminLogDto
            {
                Id = al.Id,
                UserId = al.UserId,
                Action = al.Action,
                Entity = al.Entity,
                EntityId = al.EntityId,
                Timestamp = al.Timestamp
            });
        }

        public async Task<AdminLogDto> CreateAsync(CreateAdminLogDto dto)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (string.IsNullOrWhiteSpace(dto.Action) || string.IsNullOrWhiteSpace(dto.Entity))
                throw new ArgumentException("Action and Entity are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var adminLog = new AdminLog
            {
                UserId = dto.UserId,
                Action = dto.Action,
                Entity = dto.Entity,
                EntityId = dto.EntityId,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.AdminLogs.AddAsync(adminLog);
            await _unitOfWork.CompleteAsync();

            return new AdminLogDto
            {
                Id = adminLog.Id,
                UserId = adminLog.UserId,
                Action = adminLog.Action,
                Entity = adminLog.Entity,
                EntityId = adminLog.EntityId,
                Timestamp = adminLog.Timestamp
            };
        }

        public async Task DeleteAsync(int id)
        {
            var adminLog = await _unitOfWork.AdminLogs.GetByIdAsync(id);
            if (adminLog == null)
                throw new KeyNotFoundException("Admin log not found.");

            await _unitOfWork.AdminLogs.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}