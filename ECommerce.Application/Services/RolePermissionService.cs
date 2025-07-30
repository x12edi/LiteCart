using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RolePermissionDto> GetByKeysAsync(int roleId, int permissionId)
        {
            var rolePermission = await _unitOfWork.RolePermissions.GetByRoleAndPermissionIdAsync(roleId, permissionId);
            if (rolePermission == null)
                return null;

            return new RolePermissionDto
            {
                RoleId = rolePermission.RoleId,
                PermissionId = rolePermission.PermissionId
            };
        }

        public async Task<IEnumerable<RolePermissionDto>> GetAllAsync()
        {
            var rolePermissions = await _unitOfWork.RolePermissions.GetAllAsync();
            return rolePermissions.Select(rp => new RolePermissionDto
            {
                RoleId = rp.RoleId,
                PermissionId = rp.PermissionId
            });
        }

        public async Task<RolePermissionDto> CreateAsync(CreateRolePermissionDto dto)
        {
            if (dto.RoleId <= 0 || dto.PermissionId <= 0)
                throw new ArgumentException("Valid RoleId and PermissionId are required.");

            var role = await _unitOfWork.Roles.GetByIdAsync(dto.RoleId);
            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            var permission = await _unitOfWork.Permissions.GetByIdAsync(dto.PermissionId);
            if (permission == null)
                throw new KeyNotFoundException("Permission not found.");

            var rolePermission = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId
            };

            await _unitOfWork.RolePermissions.AddAsync(rolePermission);
            await _unitOfWork.CompleteAsync();

            return new RolePermissionDto
            {
                RoleId = rolePermission.RoleId,
                PermissionId = rolePermission.PermissionId
            };
        }

        public async Task DeleteAsync(int roleId, int permissionId)
        {
            var rolePermission = await _unitOfWork.RolePermissions.GetByRoleAndPermissionIdAsync(roleId, permissionId);
            if (rolePermission == null)
                throw new KeyNotFoundException("Role permission mapping not found.");

            await _unitOfWork.RolePermissions.DeleteByRoleAndPermissionIdAsync(roleId, permissionId);
            await _unitOfWork.CompleteAsync();
        }
    }
}