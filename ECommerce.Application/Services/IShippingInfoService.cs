using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IShippingInfoService
    {
        Task<ShippingInfoDto> GetByIdAsync(int id);
        Task<IEnumerable<ShippingInfoDto>> GetAllAsync();
        Task<ShippingInfoDto> CreateAsync(CreateShippingInfoDto dto);
        Task UpdateAsync(int id, UpdateShippingInfoDto dto);
        Task DeleteAsync(int id);
    }
}