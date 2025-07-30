using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface ITaxRuleService
    {
        Task<TaxRuleDto> GetByIdAsync(int id);
        Task<IEnumerable<TaxRuleDto>> GetAllAsync();
        Task<TaxRuleDto> CreateAsync(CreateTaxRuleDto dto);
        Task UpdateAsync(int id, UpdateTaxRuleDto dto);
        Task DeleteAsync(int id);
    }
}