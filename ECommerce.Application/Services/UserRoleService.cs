using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRoleDto> GetByKeysAsync(int userId, int roleId)
        {
            var userRole = await _unitOfWork.UserRoles.GetByUserAndRoleIdAsync(userId, roleId);
            if (userRole == null)
                return null;

            return new UserRoleDto
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId
            };
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllAsync()
        {
            var userRoles = await _unitOfWork.UserRoles.GetAllAsync();
            return userRoles.Select(ur => new UserRoleDto
            {
                UserId = ur.UserId,
                RoleId = ur.RoleId
            });
        }

        public async Task<UserRoleDto> CreateAsync(CreateUserRoleDto dto)
        {
            if (dto.UserId <= 0 || dto.RoleId <= 0)
                throw new ArgumentException("Valid UserId and RoleId are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var role = await _unitOfWork.Roles.GetByIdAsync(dto.RoleId);
            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            var userRole = new UserRole
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            await _unitOfWork.UserRoles.AddAsync(userRole);
            await _unitOfWork.CompleteAsync();

            return new UserRoleDto
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId
            };
        }

        public async Task DeleteAsync(int userId, int roleId)
        {
            var userRole = await _unitOfWork.UserRoles.GetByUserAndRoleIdAsync(userId, roleId);
            if (userRole == null)
                throw new KeyNotFoundException("User role mapping not found.");

            await _unitOfWork.UserRoles.DeleteByUserAndRoleIdAsync(userId, roleId);
            await _unitOfWork.CompleteAsync();
        }
    }
}