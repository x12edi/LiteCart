using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PermissionDto> GetByIdAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
                return null;

            return new PermissionDto
            {
                Id = permission.Id,
                Action = permission.Action,
                Resource = permission.Resource
            };
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Action = p.Action,
                Resource = p.Resource
            });
        }

        public async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Action) || string.IsNullOrWhiteSpace(dto.Resource))
                throw new ArgumentException("Action and Resource are required.");

            var permission = new Permission
            {
                Action = dto.Action,
                Resource = dto.Resource
            };

            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CompleteAsync();

            return new PermissionDto
            {
                Id = permission.Id,
                Action = permission.Action,
                Resource = permission.Resource
            };
        }

        public async Task UpdateAsync(int id, UpdatePermissionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Action) || string.IsNullOrWhiteSpace(dto.Resource))
                throw new ArgumentException("Action and Resource are required.");

            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
                throw new KeyNotFoundException("Permission not found.");

            permission.Action = dto.Action;
            permission.Resource = dto.Resource;

            await _unitOfWork.Permissions.UpdateAsync(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
                throw new KeyNotFoundException("Permission not found.");

            //await _unitOfWork.RolePermissions.DeleteByPermissionIdAsync(id);
            await _unitOfWork.RolePermissions.DeleteAsync(id);
            await _unitOfWork.Permissions.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}