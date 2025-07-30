using ECommerce.Application.Dtos;

namespace ECommerce.Application.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseDto> GetByIdAsync(int id);
        Task<IEnumerable<WarehouseDto>> GetAllAsync();
        Task<WarehouseDto> CreateAsync(CreateWarehouseDto dto);
        Task UpdateAsync(int id, UpdateWarehouseDto dto);
        Task DeleteAsync(int id);
    }
}