using ECommerce.Application.Dtos;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class TaxRuleService : ITaxRuleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaxRuleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaxRuleDto> GetByIdAsync(int id)
        {
            var taxRule = await _unitOfWork.TaxRules.GetByIdAsync(id);
            if (taxRule == null)
                return null;

            return new TaxRuleDto
            {
                Id = taxRule.Id,
                Region = taxRule.Region,
                Rate = taxRule.Rate,
                Type = taxRule.Type
            };
        }

        public async Task<IEnumerable<TaxRuleDto>> GetAllAsync()
        {
            var taxRules = await _unitOfWork.TaxRules.GetAllAsync();
            return taxRules.Select(tr => new TaxRuleDto
            {
                Id = tr.Id,
                Region = tr.Region,
                Rate = tr.Rate,
                Type = tr.Type
            });
        }

        public async Task<TaxRuleDto> CreateAsync(CreateTaxRuleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Region) || string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException("Region and Type are required.");
            if (dto.Rate < 0)
                throw new ArgumentException("Rate cannot be negative.");

            var taxRule = new TaxRule
            {
                Region = dto.Region,
                Rate = dto.Rate,
                Type = dto.Type
            };

            await _unitOfWork.TaxRules.AddAsync(taxRule);
            await _unitOfWork.CompleteAsync();

            return new TaxRuleDto
            {
                Id = taxRule.Id,
                Region = taxRule.Region,
                Rate = taxRule.Rate,
                Type = taxRule.Type
            };
        }

        public async Task UpdateAsync(int id, UpdateTaxRuleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Region) || string.IsNullOrWhiteSpace(dto.Type))
                throw new ArgumentException("Region and Type are required.");
            if (dto.Rate < 0)
                throw new ArgumentException("Rate cannot be negative.");

            var taxRule = await _unitOfWork.TaxRules.GetByIdAsync(id);
            if (taxRule == null)
                throw new KeyNotFoundException("Tax rule not found.");

            taxRule.Region = dto.Region;
            taxRule.Rate = dto.Rate;
            taxRule.Type = dto.Type;

            await _unitOfWork.TaxRules.UpdateAsync(taxRule);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taxRule = await _unitOfWork.TaxRules.GetByIdAsync(id);
            if (taxRule == null)
                throw new KeyNotFoundException("Tax rule not found.");

            await _unitOfWork.TaxRules.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}