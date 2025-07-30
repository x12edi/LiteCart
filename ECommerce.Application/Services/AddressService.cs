using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressDto> GetByIdAsync(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
                return null;

            return new AddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                State = address.State,
                Zip = address.Zip,
                Country = address.Country,
                IsDefault = address.IsDefault
            };
        }

        public async Task<IEnumerable<AddressDto>> GetAllAsync()
        {
            var addresses = await _unitOfWork.Addresses.GetAllAsync();
            return addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                UserId = a.UserId,
                Street = a.Street,
                City = a.City,
                State = a.State,
                Zip = a.Zip,
                Country = a.Country,
                IsDefault = a.IsDefault
            });
        }

        public async Task<AddressDto> CreateAsync(CreateAddressDto dto)
        {
            if (dto.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (string.IsNullOrWhiteSpace(dto.Street) || string.IsNullOrWhiteSpace(dto.City) ||
                string.IsNullOrWhiteSpace(dto.State) || string.IsNullOrWhiteSpace(dto.Zip) ||
                string.IsNullOrWhiteSpace(dto.Country))
                throw new ArgumentException("All address fields are required.");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var address = new Address
            {
                UserId = dto.UserId,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                Zip = dto.Zip,
                Country = dto.Country,
                IsDefault = dto.IsDefault
            };

            // If setting as default, unset other defaults for the user
            if (dto.IsDefault)
            {
                var userAddresses = await _unitOfWork.Addresses.GetAllAsync();
                foreach (var addr in userAddresses.Where(a => a.UserId == dto.UserId && a.IsDefault))
                {
                    addr.IsDefault = false;
                    await _unitOfWork.Addresses.UpdateAsync(addr);
                }
            }

            await _unitOfWork.Addresses.AddAsync(address);
            await _unitOfWork.CompleteAsync();

            return new AddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                State = address.State,
                Zip = address.Zip,
                Country = address.Country,
                IsDefault = address.IsDefault
            };
        }

        public async Task UpdateAsync(int id, UpdateAddressDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Street) || string.IsNullOrWhiteSpace(dto.City) ||
                string.IsNullOrWhiteSpace(dto.State) || string.IsNullOrWhiteSpace(dto.Zip) ||
                string.IsNullOrWhiteSpace(dto.Country))
                throw new ArgumentException("All address fields are required.");

            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
                throw new KeyNotFoundException("Address not found.");

            address.Street = dto.Street;
            address.City = dto.City;
            address.State = dto.State;
            address.Zip = dto.Zip;
            address.Country = dto.Country;
            address.IsDefault = dto.IsDefault;

            // If setting as default, unset other defaults for the user
            if (dto.IsDefault)
            {
                var userAddresses = await _unitOfWork.Addresses.GetAllAsync();
                foreach (var addr in userAddresses.Where(a => a.UserId == address.UserId && a.IsDefault && a.Id != id))
                {
                    addr.IsDefault = false;
                    await _unitOfWork.Addresses.UpdateAsync(addr);
                }
            }

            await _unitOfWork.Addresses.UpdateAsync(address);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
                throw new KeyNotFoundException("Address not found.");

            await _unitOfWork.Addresses.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}