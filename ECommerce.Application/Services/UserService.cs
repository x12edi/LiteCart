using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Name,
                //IsActive = user.IsActive
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Name,
                //IsActive = u.IsActive
            });
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Email and username are required.");
            if (string.IsNullOrWhiteSpace(dto.PasswordHash))
                throw new ArgumentException("Password hash is required.");

            var user = new User
            {
                Email = dto.Email,
                Name = dto.Username,
                PasswordHash = dto.PasswordHash
                //IsActive = dto.IsActive
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Name
                //IsActive = user.IsActive
            };
        }

        public async Task UpdateAsync(int id, UpdateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Email and username are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.Email = dto.Email;
            user.Name = dto.Username;
            //user.IsActive = dto.IsActive;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}