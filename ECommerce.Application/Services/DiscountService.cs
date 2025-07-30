using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DiscountDto> GetByIdAsync(int id)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(id);
            if (discount == null)
                return null;

            return new DiscountDto
            {
                Id = discount.Id,
                Code = discount.Code,
                //Percentage = discount.Percentage,
                //ValidFrom = discount.ValidFrom,
                //ValidUntil = discount.ValidUntil,
                //MaxUses = discount.MaxUses,
                //Uses = discount.Uses
            };
        }

        public async Task<IEnumerable<DiscountDto>> GetAllAsync()
        {
            var discounts = await _unitOfWork.Discounts.GetAllAsync();
            return discounts.Select(d => new DiscountDto
            {
                Id = d.Id,
                Code = d.Code,
                //Percentage = d.Percentage,
                //ValidFrom = d.ValidFrom,
                //ValidUntil = d.ValidUntil,
                //MaxUses = d.MaxUses,
                //Uses = d.Uses
            });
        }

        public async Task<DiscountDto> CreateAsync(CreateDiscountDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Discount code is required.");
            if (dto.Percentage < 0 || dto.Percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100.");
            if (dto.ValidUntil < dto.ValidFrom)
                throw new ArgumentException("ValidUntil must be after ValidFrom.");
            if (dto.MaxUses.HasValue && dto.MaxUses <= 0)
                throw new ArgumentException("MaxUses must be positive.");

            var discount = new Discount
            {
                Code = dto.Code,
                //Percentage = dto.Percentage,
                //ValidFrom = dto.ValidFrom,
                //ValidUntil = dto.ValidUntil,
                //MaxUses = dto.MaxUses,
                //Uses = 0
            };

            await _unitOfWork.Discounts.AddAsync(discount);
            await _unitOfWork.CompleteAsync();

            return new DiscountDto
            {
                Id = discount.Id,
                Code = discount.Code,
                //Percentage = discount.Percentage,
                //ValidFrom = discount.ValidFrom,
                //ValidUntil = discount.ValidUntil,
                //MaxUses = discount.MaxUses,
                //Uses = discount.Uses
            };
        }

        public async Task UpdateAsync(int id, UpdateDiscountDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new ArgumentException("Discount code is required.");
            if (dto.Percentage < 0 || dto.Percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100.");
            if (dto.ValidUntil < dto.ValidFrom)
                throw new ArgumentException("ValidUntil must be after ValidFrom.");
            if (dto.MaxUses.HasValue && dto.MaxUses <= 0)
                throw new ArgumentException("MaxUses must be positive.");

            var discount = await _unitOfWork.Discounts.GetByIdAsync(id);
            if (discount == null)
                throw new KeyNotFoundException("Discount not found.");

            discount.Code = dto.Code;
            //discount.Percentage = dto.Percentage;
            //discount.ValidFrom = dto.ValidFrom;
            //discount.ValidUntil = dto.ValidUntil;
            //discount.MaxUses = dto.MaxUses;
            //discount.Uses = dto.Uses;

            await _unitOfWork.Discounts.UpdateAsync(discount);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(id);
            if (discount == null)
                throw new KeyNotFoundException("Discount not found.");

            await _unitOfWork.Discounts.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}