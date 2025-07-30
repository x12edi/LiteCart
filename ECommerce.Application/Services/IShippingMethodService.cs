using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IShippingMethodService
    {
        Task<ShippingMethodDto> GetByIdAsync(int id);
        Task<IEnumerable<ShippingMethodDto>> GetAllAsync();
        Task<ShippingMethodDto> CreateAsync(CreateShippingMethodDto dto);
        Task UpdateAsync(int id, UpdateShippingMethodDto dto);
        Task DeleteAsync(int id);
    }
}