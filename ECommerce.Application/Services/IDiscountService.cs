using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IDiscountService
    {
        Task<DiscountDto> GetByIdAsync(int id);
        Task<IEnumerable<DiscountDto>> GetAllAsync();
        Task<DiscountDto> CreateAsync(CreateDiscountDto dto);
        Task UpdateAsync(int id, UpdateDiscountDto dto);
        Task DeleteAsync(int id);
    }
}