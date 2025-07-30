using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
                return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Role name is required.");

            var role = new Role
            {
                Name = dto.Name
            };

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CompleteAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task UpdateAsync(int id, UpdateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Role name is required.");

            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            role.Name = dto.Name;

            await _unitOfWork.Roles.UpdateAsync(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
            //var role = await _unitOfWork.Roles.GetByIdAsync(id);
            //if (role == null)
            //    throw new KeyNotFoundException("Role not found.");

            //await _unitOfWork.RolePermissions.DeleteByRoleIdAsync(id);
            //await _unitOfWork.UserRoles.DeleteByRoleIdAsync(id);
            //await _unitOfWork.Roles.DeleteAsync(id);
            //await _unitOfWork.CompleteAsync();
        }
    }
}